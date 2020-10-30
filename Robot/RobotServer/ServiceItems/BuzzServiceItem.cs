using System;
using Microsoft.Extensions.Logging;
using Robot.Devices;
using RobotControllerContract;
using RobotServer.Services;

namespace RobotServer.ServiceItems
{
    public interface IBuzzServiceItem
    {
        Reply Buzz(BuzzValue request);
    }

    public class BuzzServiceItem : ServiceItemBase, IBuzzServiceItem
    {
        public IBuzzer _buzzer;
        
        public BuzzServiceItem(ILogger<RobotService> logger, IBuzzer buzzer):base(logger)
        {
            _buzzer = buzzer;
        }
        
        public Reply Buzz(BuzzValue request)
        {
            try
            {
                if(request.OnOff)
                    _buzzer.Start();
                else
                {
                    _buzzer.Stop();
                }

                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error in buzzer");
                return new Reply() {Success = false};
            }
        }
    }
}