using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemanticsWEB.Services;

namespace SemanticsWEB.Controllers
{
    [Route("api/financial")]
    public class FinancialDataController : Controller
    {

        private readonly ILogger<FinancialDataController> _logger;
        private readonly IRdfQueryService _rdfQueryService;

        public FinancialDataController(ILogger<FinancialDataController> logger, IRdfQueryService rdfQueryService)
        {
            _logger = logger;
            _rdfQueryService = rdfQueryService;
        }
        
        [HttpGet()]
        public IActionResult QueryCurrencies()
        {
            _logger.LogInformation("i was here");
            return Ok(_rdfQueryService.Query());
        }
    }
}