using Microsoft.Extensions.Logging;
using RobotServer.Services;

namespace RobotServer.ServiceItems
{
    public abstract class ServiceItemBase
    {
        protected ILogger<RobotService> _logger;

        public ServiceItemBase(ILogger<RobotService> logger)
        {
            _logger = logger;
        }
    }
}