using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using SemanticsWEB.Extensions;
using SemanticsWEB.Models;
using VDS.RDF;
using VDS.RDF.Query;
using VDS.RDF.Storage;
using Graph = SemanticsWEB.Models.Graph;
using Triple = SemanticsWEB.Models.Triple;

namespace SemanticsWEB.Repositories
{
    public class SesameRepository : ISesameRepository
    {
        private const string Rdf4jEndpoint = "http://192.168.33.10:8080/rdf4j-server/";

        private const string RepositoryId = "infcurr";

        private readonly ILogger<SesameRepository> _logger;

        public SesameRepository(ILogger<SesameRepository> logger)
        {
            _logger = logger;
        }
        
        public IEnumerable<Triple> Query()
        {
            
            _logger.LogInformation("query rdf financial store");

            const string query = "PREFIX tr-common: <http://permid.org/ontology/common/>\n" +
                                 "PREFIX tr-fin: <http://permid.org/ontology/financial/>\n" +
                                 "PREFIX tr: <http://www.thomsonreuters.com/>\n" +
                                 "PREFIX permid: <https://permid.org/>\n" +
                                 "SELECT ?subject ?predicate ?object\n" +
                                 "WHERE {\n" +
                                 "BIND(permid:1-1000285985 AS ?subject) .\n" +
                                 "permid:1-1000285985 ?predicate ?object" +
                                 "}\n" +
                                 "LIMIT 50";
            
            //var endpoint = new SparqlRemoteEndpoint(new Uri(Rdf4jEndpoint), Rdf4jGraphUri);
            
            var sesame = new SesameHttpProtocolConnector(Rdf4jEndpoint, RepositoryId);

            var results = sesame.Query(query) as SparqlResultSet;

            //var results = endpoint.QueryWithResultSet(query);
            
            _logger.LogInformation("got results {@results}", results);

            var resultList = new List<Triple>(); 
            
            _logger.LogInformation("queried entries: " + results.Count);
            
            foreach (var result in results)
            {
                var subject = result["subject"];
                var predicate = result["predicate"];
                var obj = result["object"];
                
                resultList.Add(new Triple()
                {
                    Subject = result["subject"].ToString(),
                    Predicate = result["predicate"].ToString(),
                    Object = result["object"].ToString()
                });
                
                //_logger.LogInformation("{@subject} {@predicate} {obj}", subject, predicate, obj);
            }

            return resultList;
        }

        public Graph QueryGraph()
        {
            var nodeDictionary = new Dictionary<string, Node>();
            var edgeSet = new HashSet<Edge>();

            var nodeCounter = 0;
            var triples = Query();
            foreach (var triple in triples)
            {
                var subject = triple.Subject;
                var obj = triple.Object;
                var subjectNode = nodeDictionary.ComputeIfAbsent(subject, key => new Node(nodeCounter++, subject));
                var objectNode = nodeDictionary.ComputeIfAbsent(obj, key => new Node(nodeCounter++, obj));

                edgeSet.Add(new Edge(subjectNode.Id, objectNode.Id, triple.Predicate));
            }
            
            return new Graph(nodeDictionary.Values, edgeSet);
        }
    }
}