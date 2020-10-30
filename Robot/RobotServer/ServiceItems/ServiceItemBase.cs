using RobotServer.Services;
using Microsoft.Extensions.Logging;

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