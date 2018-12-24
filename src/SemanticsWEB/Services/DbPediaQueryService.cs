using Microsoft.Extensions.Logging;

namespace SemanticsWEB.Services
{
    public class DbPediaQueryService : IDbPediaQueryService
    {
        private ILogger<DbPediaQueryService> _logger;

        public DbPediaQueryService(ILogger<DbPediaQueryService> logger)
        {
            _logger = logger;
        }
        
        public void doIt()
        {
            _logger.LogInformation("quering dbpedia");
        }
    }
}