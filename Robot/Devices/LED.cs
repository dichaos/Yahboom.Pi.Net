using System.Device.Gpio;
using Robot.Configs;

namespace Robot.Devices
{
    public class LED
    {
        private GpioController _controller;
        private LEDSettings  _ledSettings;
        
        public LED(LEDSettings ledSettings, GpioController controller)
        {
            _ledSettings = ledSettings;
            _controller = controller;
            
            _controller.OpenPin(_ledSettings.Red, PinMode.Output);
            _controller.OpenPin(_ledSettings.Blue, PinMode.Output);
            _controller.OpenPin(_ledSettings.Green, PinMode.Output);
        }

        public void SetRGB(int red, int blue, int green)
        {
            red = GetInRange(red);
            blue = GetInRange(blue);
            green = GetInRange(green);
            
            _controller.Write(_ledSettings.Red, red);
            _controller.Write(_ledSettings.Green, green);
            _controller.Write(_ledSettings.Blue, blue);
        }

        private int GetInRange(int value)
        {
            value = (value * (2500 - 500)) / 255;

            if (value > 0 && value < 500)
                value = 500;
            else if(value > 2500)
                value = 2500;

            return value;
        }
    }
}