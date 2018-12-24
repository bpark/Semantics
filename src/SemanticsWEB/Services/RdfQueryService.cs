using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SemanticsWEB.Models;
using VDS.RDF.Query;
using VDS.RDF.Storage;

namespace SemanticsWEB.Services
{
    public class RdfQueryService : IRdfQueryService
    {
        private const string Rdf4jEndpoint = "http://192.168.33.10:8080/rdf4j-server/";

        private const string RepositoryId = "financials";

        private readonly ILogger<RdfQueryService> _logger;

        public RdfQueryService(ILogger<RdfQueryService> logger)
        {
            _logger = logger;
        }
        
        public IEnumerable<Triple> Query()
        {
            
            _logger.LogInformation("query rdf financial store");
            
            const string query = "PREFIX tr-common: <http://permid.org/ontology/common/>\n" +
                                 "PREFIX tr-fin: <http://permid.org/ontology/financial/>\n" +
                                 "PREFIX tr: <http://www.thomsonreuters.com/>\n" +
                                 "SELECT ?subject ?predicate ?object\n" +
                                 "WHERE {\n" +
                                 "?subject ?predicate ?object\n" +
                                 "}\n" +
                                 "LIMIT 10";
            
            //var endpoint = new SparqlRemoteEndpoint(new Uri(Rdf4jEndpoint), Rdf4jGraphUri);
            
            var sesame = new SesameHttpProtocolConnector(Rdf4jEndpoint, RepositoryId);

            var results = sesame.Query(query) as SparqlResultSet;

            //var results = endpoint.QueryWithResultSet(query);
            
            _logger.LogInformation("got results {@results}", results);

            var resultList = new List<Triple>(); 
            
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
                
                _logger.LogInformation("{@subject} {@predicate} {obj}", subject, predicate, obj);
            }

            return resultList;
        }
    }
}