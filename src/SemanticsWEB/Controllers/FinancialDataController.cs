using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemanticsWEB.Repositories;
using VDS.RDF;

namespace SemanticsWEB.Controllers
{
    [Route("api/financial")]
    public class FinancialDataController : Controller
    {
        private const string DefaultResource = "permid:1-1003939166";

        private readonly ILogger<FinancialDataController> _logger;
        private readonly ISesameRepository _sesameRepository;

        public FinancialDataController(ILogger<FinancialDataController> logger, ISesameRepository sesameRepository)
        {
            _logger = logger;
            _sesameRepository = sesameRepository;
        }

        [HttpGet]
        public IActionResult QueryCurrencies()
        {
            return Ok(_sesameRepository.QueryResource(NodeType.Uri, DefaultResource));
        }

        [HttpGet("resource")]
        public IActionResult QueryResource(NodeType nodeType, string resource)
        {
            _logger.LogInformation("querying resource {@nodeType}/{@resource}", nodeType, resource);
            return Ok(_sesameRepository.QueryResource(nodeType, resource));
        }
    }
}