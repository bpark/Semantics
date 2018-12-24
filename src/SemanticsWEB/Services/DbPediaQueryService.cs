using System;
using Microsoft.Extensions.Logging;
using VDS.RDF.Query;

namespace SemanticsWEB.Services
{
    public class DbPediaQueryService : IDbPediaQueryService
    {
        private static readonly string DbPediaEndpoint = "http://dbpedia.org/sparql";

        private static readonly string DbPediaGraphUri = "http://dbpedia.org";
        
        private static readonly string[] Prefixes = {
            "PREFIX owl: <http://www.w3.org/2002/07/owl#>",
            "PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>",
            "PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>",
            "PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>",
            "PREFIX foaf: <http://xmlns.com/foaf/0.1/>",
            "PREFIX dc: <http://purl.org/dc/elements/1.1/>",
            "PREFIX : <http://dbpedia.org/resource/>",
            "PREFIX dbpedia2: <http://dbpedia.org/property/>",
            "PREFIX dbpedia: <http://dbpedia.org/>",
            "PREFIX skos: <http://www.w3.org/2004/02/skos/core#>",
            "PREFIX ont: <http://dbpedia.org/ontology/>"
        }; 
        
        private readonly ILogger<DbPediaQueryService> _logger;

        public DbPediaQueryService(ILogger<DbPediaQueryService> logger)
        {
            _logger = logger;
        }
        
        public void doIt()
        {
            _logger.LogInformation("quering dbpedia");
            
            var prefix = string.Join("\n", Prefixes);
            
            var query = prefix + "\nSELECT ?comment WHERE {\n" +
                        "?body a ont:CelestialBody .\n" +
                        "?body foaf:name \"Vega\"@en .\n" +
                        "?body rdfs:comment ?comment .\n" +
                        "FILTER ( lang(?comment) = \"en\")\n" +
                        "}";
            
            //Define a remote endpoint
            //Use the DBPedia SPARQL endpoint with the default Graph set to DBPedia
            var endpoint = new SparqlRemoteEndpoint(new Uri(DbPediaEndpoint), DbPediaGraphUri);

            //Make a SELECT query against the Endpoint
            //var results = endpoint.QueryWithResultSet("SELECT DISTINCT ?Concept WHERE {[] a ?Concept}");
            var results = endpoint.QueryWithResultSet(query);
            foreach (var result in results)
            {
                var comment = result["comment"];
                _logger.LogInformation("queried result {@comment}", comment);
            }
        }
    }
}