using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SemanticsWEB.Extensions;
using SemanticsWEB.Models;
using SemanticsWEB.Utils;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Storage;
using VDS.RDF.Writing;
using static System.String;
using Graph = SemanticsWEB.Models.Graph;

namespace SemanticsWEB.Repositories
{
    public class SesameRepository : ISesameRepository
    {
        /// <summary>
        /// The rdf4j server data endpoint.
        /// </summary>
        private const string Rdf4JEndpoint = "http://192.168.33.10:8080/rdf4j-server/";

        /// <summary>
        /// The repository to query.
        /// </summary>
        private const string RepositoryId = "geocurrencies";

        private readonly ILogger<SesameRepository> _logger;

        /// <summary>
        /// List of used sparql prefixes. URI parts are replaced by the prefixes.
        /// </summary>
        private static readonly Dictionary<string, string> Prefixes = new Dictionary<string, string>()
        {
            {"tr-common", "http://permid.org/ontology/common/"},
            {"tr-fin", "http://permid.org/ontology/financial/"},
            {"tr", "http://www.thomsonreuters.com/"},
            {"tr-curr", "http://permid.org/ontology/currency/"},
            {"rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns"},
            {"rdfs", "http://www.w3.org/2000/01/rdf-schema#"},
            {"skos", "http://www.w3.org/2004/02/skos/core#"},
            {"xsd", "http://www.w3.org/2001/XMLSchema#"},
            {"owl", "http://www.w3.org/2002/07/owl#"},
            {"permid", "https://permid.org/"},
            {"gn", "http://www.geonames.org/ontology#"}
        };

        /// <summary>
        /// The sparql query used to query rdf statements.
        /// </summary>
        private static readonly string SparqlQuery =
            @"SELECT ?subject ?predicate ?object
              WHERE {
                 ?subject ?predicate ?object
                 FILTER (?subject = @value || ?predicate = @value || ?object = @value) .
                 FILTER (langMatches(lang(?object), """") || langMatches(lang(?object), ""EN"") || ISURI(?object))
              }
              LIMIT 50";

        //                  FILTER (langMatches(lang(?object), "") || langMatches(lang(?object), "EN")) 

        public SesameRepository(ILogger<SesameRepository> logger)
        {
            _logger = logger;
        }

        public Graph QueryResource(NodeType nodeType, string resource)
        {
            var results = QuerySparql(nodeType, resource);

            var nodeDictionary = new Dictionary<string, Node>();
            var edgeSet = new HashSet<Edge>();

            foreach (var result in results)
            {
                var subjectValue = GetResultValue(result, "subject");
                var predicateValue = GetResultValue(result, "predicate");
                var objectValue = GetResultValue(result, "object");

                var subjectNode = nodeDictionary.ComputeIfAbsent(subjectValue, CreateNodeFunc(subjectValue));
                var objectNode = nodeDictionary.ComputeIfAbsent(objectValue, CreateNodeFunc(objectValue));

                var toHash = subjectNode.Id + "/" + objectNode.Id;
                var id = Murmur3.Hash(toHash).ToString();
                edgeSet.Add(new Edge(id, subjectNode.Id, objectNode.Id, CreateNamespaceString(predicateValue)));
            }

            return new Graph(nodeDictionary.Values, edgeSet);
        }

        private static Func<string, Node> CreateNodeFunc(string value)
        {
            var nodeType = EvaluateNodeType(value);
            var namespaceString = CreateNamespaceString(value);
            var id = Murmur3.Hash(value).ToString();

            return key => new Node(id, nodeType, namespaceString);
        }

        private static string CreateNamespaceString(string uriString)
        {
            var namespaceString = uriString;
            foreach (var (key, value) in Prefixes)
            {
                namespaceString = namespaceString.Replace(value, key + ":");
            }

            return namespaceString;
        }

        private static string CreateUriString(string namespaceString)
        {
            var uriString = namespaceString;
            foreach (var (key, value) in Prefixes)
            {
                uriString = uriString.Replace(key + ":", value);
            }

            return uriString;
        }

        private SparqlResultSet QuerySparql(NodeType nodeType, string resource)
        {
            _logger.LogInformation("query rdf financial store");

            var query = CreateQuery(nodeType, resource);

            _logger.LogInformation("query: {@query}", query);

            var sesame = new SesameHttpProtocolConnector(Rdf4JEndpoint, RepositoryId);

            var results = sesame.Query(query) as SparqlResultSet;

            _logger.LogInformation("got results {@results}", results);

            return results;
        }

        private static string CreateQuery(NodeType nodeType, string resource)
        {
            var queryString = new SparqlParameterizedString();

            foreach (var (key, value) in Prefixes)
            {
                queryString.Namespaces.AddNamespace(key, new Uri(value));
            }

            queryString.CommandText = SparqlQuery;

            if (nodeType == NodeType.Uri)
            {
                var uriString = CreateUriString(resource);
                queryString.SetUri("value", new Uri(uriString));
            }
            else
            {
                queryString.SetLiteral("value", resource);
            }

            return queryString.ToString();
        }


        private static string GetResultValue(SparqlResult result, string variable)
        {
            string data;
            if (result.TryGetValue(variable, out var n))
            {
                switch (n.NodeType)
                {
                    case NodeType.Uri:
                        data = ((IUriNode) n).Uri.AbsoluteUri;
                        break;
                    case NodeType.Blank:
                        data = ((IBlankNode) n).InternalID;
                        break;
                    case NodeType.Literal:
                        data = ((ILiteralNode) n).Value;
                        break;
                    case NodeType.GraphLiteral:
                        data = n.ToString();
                        break;
                    case NodeType.Variable:
                        data = n.ToString();
                        break;
                    default:
                        throw new RdfOutputException("Unexpected Node Type");
                }
            }
            else
            {
                data = Empty;
            }

            return data;
        }

        private static NodeType EvaluateNodeType(string value)
        {
            if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || 
                value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                return NodeType.Uri;
            }
            else
            {
                return NodeType.Literal;
            }
        }

    }
}