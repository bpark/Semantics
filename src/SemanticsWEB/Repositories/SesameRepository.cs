using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SemanticsWEB.Extensions;
using SemanticsWEB.Models;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Storage;
using VDS.RDF.Writing;
using static System.String;
using Graph = SemanticsWEB.Models.Graph;
using Triple = SemanticsWEB.Models.Triple;

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
        private const string RepositoryId = "currencies";
        //private const string RepositoryId = "infcurr";

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
            {"permid", "https://permid.org/"}
        };

        public SesameRepository(ILogger<SesameRepository> logger)
        {
            _logger = logger;
        }
        
        public IEnumerable<Triple> Query()
        {
            var results = QuerySparql(NodeType.Uri, "permid:1-1003939166");
            
            var resultList = new List<Triple>(); 
            
            _logger.LogInformation("queried entries: " + results.Count);
            
            foreach (var result in results)
            {
                
                resultList.Add(new Triple()
                {
                    Subject = GetResultValue(result, "subject"),
                    Predicate = GetResultValue(result, "predicate"),
                    Object = GetResultValue(result, "object")
                });
                
            }

            return resultList;
        }

        public Graph QueryResource(NodeType nodeType, string resource)
        {
            var results = QuerySparql(nodeType, resource);
            
            var nodeDictionary = new Dictionary<string, Node>();
            var edgeSet = new HashSet<Edge>();

            var nodeCounter = 0;
            foreach (var result in results)
            {

                var subjectValue = GetResultValue(result, "subject");
                var predicateValue = GetResultValue(result, "predicate");
                var objectValue = GetResultValue(result, "object");
                
                var subjectNode = nodeDictionary.ComputeIfAbsent(subjectValue, CreateNodeFunc(subjectValue, nodeCounter++));
                var objectNode = nodeDictionary.ComputeIfAbsent(objectValue, CreateNodeFunc(objectValue, nodeCounter++));

                edgeSet.Add(new Edge(subjectNode.Id, objectNode.Id, CreateNamespaceString(predicateValue)));
            }
            
            return new Graph(nodeDictionary.Values, edgeSet);
        }

        private static Func<string, Node> CreateNodeFunc(string value, int nodeCounter)
        {
            var nodeType = EvaluateNodeType(value);
            var namespaceString = CreateNamespaceString(value);
            
            return key => new Node(nodeCounter, nodeType, namespaceString);
        }

        private static string CreateNamespaceString(string uriString)
        {
            var namespaceString = uriString;
            foreach(var (key, value) in Prefixes)
            {
                namespaceString = namespaceString.Replace(value, key + ":");
            }
            
            return namespaceString;
        }

        private static string CreateUriString(string namespaceString)
        {
            var uriString = namespaceString;
            foreach(var (key, value) in Prefixes)
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

            foreach(var (key, value) in Prefixes)
            {
                queryString.Namespaces.AddNamespace(key, new Uri(value));
            }

            queryString.CommandText = "SELECT ?subject ?predicate ?object\n" +
                                      "WHERE {\n" +
                                      "?subject ?predicate ?object\n" +
                                      "FILTER (?subject = @permid || ?predicate = @permid || ?object = @permid)\n" +
                                      "}\n" +
                                      "LIMIT 50";
            
            if (nodeType == NodeType.Uri)
            {
                var uriString = CreateUriString(resource);
                queryString.SetUri("permid", new Uri(uriString));                     
            }
            else
            {
                queryString.SetLiteral("permid", resource);
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
                        data = ((IUriNode)n).Uri.AbsoluteUri;
                        break;
                    case NodeType.Blank:
                        data = ((IBlankNode)n).InternalID;
                        break;
                    case NodeType.Literal:
                        data = ((ILiteralNode)n).Value;
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
            if (value.StartsWith("http://") || value.StartsWith("https://"))
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