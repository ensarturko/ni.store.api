using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Ni.Store.Api.Services
{
    public class HealthCheckService : IHealthCheck
    {
        private readonly ILogger<HealthCheckService> _logger;
        private readonly IStoreService _storeService;

        public HealthCheckService(
            ILogger<HealthCheckService> logger,
            IStoreService storeService)
        {
            _logger = logger;
            _storeService = storeService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var _ = await _storeService.GetFirst();

                return new HealthCheckResult(HealthStatus.Healthy);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(CheckHealthAsync)} an error occured.");

                return new HealthCheckResult(HealthStatus.Unhealthy);
            }
        }
    }
}
