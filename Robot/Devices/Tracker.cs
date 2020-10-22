using System.Device.Gpio;
using Robot.Configs;

namespace Robot.Devices
{
    public class Tracker
    {
        private readonly GpioController _controller;
        private readonly TrackerSettings  _trackerSettings;

        public Tracker(TrackerSettings settings, GpioController controller)
        {
            _controller = controller;
            _trackerSettings = settings;
            
            controller.OpenPin(_trackerSettings.LeftPin1, PinMode.Input);
            controller.OpenPin(_trackerSettings.LeftPin2, PinMode.Input);
            controller.OpenPin(_trackerSettings.RightPin1, PinMode.Input);
            controller.OpenPin(_trackerSettings.RightPin2, PinMode.Input);
        }

        public string ReadValue()
        {
            var leftPin1 = _controller.Read(_trackerSettings.LeftPin1);
            var leftPin2 = _controller.Read(_trackerSettings.LeftPin2);
            var rightPin1 = _controller.Read(_trackerSettings.RightPin1);
            var rightPin2 = _controller.Read(_trackerSettings.RightPin2);

            return leftPin1 + "," + leftPin2 + "," + rightPin1 + "," + rightPin2;
        }
    }
}