using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemanticsWEB.Handlers;
using SemanticsWEB.Repositories;
using VDS.RDF;

namespace SemanticsWEB.Controllers
{
    [Route("api/financial")]
    public class FinancialDataController : Controller
    {
        private const string DefaultResource = "permid:1-1003939166";

        private readonly ILogger<FinancialDataController> _logger;
        private readonly ResourceQueryHandler _resourceQueryHandler;

        public FinancialDataController(ILogger<FinancialDataController> logger, ResourceQueryHandler resourceQueryHandler)
        {
            _logger = logger;
            _resourceQueryHandler = resourceQueryHandler;
        }

        [HttpGet]
        public IActionResult QueryCurrencies()
        {
            _logger.LogInformation("Querying Currencies");
            return Ok(_resourceQueryHandler.Handle(new ResourceQuery(NodeType.Uri, DefaultResource)));
        }

        [HttpGet("resource")]
        public IActionResult QueryResource(NodeType nodeType, string resource)
        {
            _logger.LogInformation("Querying Resources");
            return Ok(_resourceQueryHandler.Handle(new ResourceQuery(nodeType, resource)));
        }
    }
}