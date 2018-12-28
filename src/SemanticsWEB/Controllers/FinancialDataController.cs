using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemanticsWEB.Repositories;

namespace SemanticsWEB.Controllers
{
    [Route("api/financial")]
    public class FinancialDataController : Controller
    {

        private readonly ILogger<FinancialDataController> _logger;
        private readonly ISesameRepository _sesameRepository;

        public FinancialDataController(ILogger<FinancialDataController> logger, ISesameRepository sesameRepository)
        {
            _logger = logger;
            _sesameRepository = sesameRepository;
        }
        
        [HttpGet()]
        public IActionResult QueryCurrencies()
        {
            _logger.LogInformation("i was here");
            return Ok(_sesameRepository.QueryGraph());
        }
    }
}