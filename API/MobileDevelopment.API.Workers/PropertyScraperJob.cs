using Microsoft.Extensions.Logging;
using MobileDevelopment.API.Services.Interfaces.Scraper;

namespace MobileDevelopment.API.Workers
{
    public sealed class PropertyScraperJob
    {
        private readonly ILogger<PropertyScraperJob> _logger;
        private readonly IScraperService _scraperService;

        public PropertyScraperJob(ILogger<PropertyScraperJob> logger, IScraperService scraperService)
        {
            _logger = logger;
            _scraperService = scraperService;
        }

        public async Task ExecuteAsync()
        {
            try
            {

                // logika scrapowania

                _logger.LogInformation("Scraping done successfully.");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
