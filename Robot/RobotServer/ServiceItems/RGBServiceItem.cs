using System;
using Robot.Devices;
using RobotControllerContract;

namespace RobotServer.ServiceItems
{
    public interface IRGBServiceItem
    {
        Reply LED(LEDValue request);
    }

    public class RGBServiceItem : IRGBServiceItem
    {
        private readonly ILED _led;

        public RGBServiceItem(ILED led)
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
                Console.WriteLine(ex);
                return new Reply() {Success = false};
            }
        }
    }
}