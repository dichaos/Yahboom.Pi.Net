using System;
using Microsoft.Extensions.Logging;
using Robot.Devices;
using RobotControllerContract;
using RobotServer.Services;

namespace RobotServer.ServiceItems
{
    public interface IRGBServiceItem
    {
        Reply LED(LEDValue request);
    }

    public class RGBServiceItem : ServiceItemBase, IRGBServiceItem
    {
        private readonly ILED _led;

        public RGBServiceItem(ILogger<RobotService> logger, ILED led):base(logger)
        {
            _led = led;
        }
        
        public Reply LED(LEDValue request)
        {
            try
            {
                _led.SetRGB(request.Red, request.Blue, request.Green);
                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error setting RGB");
                return new Reply() {Success = false};
            }
        }
    }
}