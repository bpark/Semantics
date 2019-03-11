using Microsoft.Extensions.Logging;
using SemanticsWEB.Models;
using SemanticsWEB.Repositories;
using VDS.RDF;
using Graph = SemanticsWEB.Models.Graph;

namespace SemanticsWEB.Handlers
{
    public sealed class ResourceQuery : IQuery<Graph>
    {
        public ResourceQuery(NodeType nodeType, string resource)
        {
            NodeType = nodeType;
            Resource = resource;
        }

        public NodeType NodeType { get; }
        public string Resource { get; }
    }

    public sealed class ResourceQueryHandler : IQueryHandler<ResourceQuery, Graph>
    {

        private readonly ISesameRepository _sesameRepository;
        private readonly ILogger<ResourceQueryHandler> _logger;

        public ResourceQueryHandler(ILogger<ResourceQueryHandler> logger, ISesameRepository sesameRepository)
        {
            _logger = logger;
            _sesameRepository = sesameRepository;
        }

        public Graph Handle(ResourceQuery query)
        {
            _logger.LogInformation("querying resource {@nodeType}/{@resource}", query.NodeType, query.Resource);
            return _sesameRepository.QueryResource(query.NodeType, query.Resource);    
        }
    }
}