using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ContaVida.DataRefresh.Services
{
    public class DataRefreshService : IDataRefreshService
    {

        private readonly ILogger<DataRefreshService> _logger;

        public DataRefreshService(ILogger<DataRefreshService> logger)
        {
                _logger = logger;
        }
        public async Task RunDataRefresh()
        {
            _logger.LogWarning("Mirror synchronization started. Turning ON Maintenance page");

            // Sync logic here

            _logger.LogInformation("Mirror synchronization finished.");
        }
    }
}
