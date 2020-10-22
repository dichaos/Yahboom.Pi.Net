using System;
using System.Device.Gpio;
using Robot.Configs;

namespace Robot.Devices
{
    public interface ITracker
    {
        Tracker.TrackerData ReadValue();
    }

    public class Tracker : ITracker
    {
        private readonly GpioController _controller;
        private readonly TrackerSettings  _trackerSettings;

        public class TrackerData
        {
            public bool LeftPin1;
            public bool LeftPin2;
            public bool RightPin1;
            public bool RightPin2;
        }

        public Tracker(TrackerSettings settings, GpioController controller)
        {
            _controller = controller;
            _trackerSettings = settings;
            
            controller.OpenPin(_trackerSettings.LeftPin1, PinMode.Input);
            controller.OpenPin(_trackerSettings.LeftPin2, PinMode.Input);
            controller.OpenPin(_trackerSettings.RightPin1, PinMode.Input);
            controller.OpenPin(_trackerSettings.RightPin2, PinMode.Input);
        }

        public TrackerData ReadValue()
        {
            var leftPin1 = _controller.Read(_trackerSettings.LeftPin1);
            var leftPin2 = _controller.Read(_trackerSettings.LeftPin2);
            var rightPin1 = _controller.Read(_trackerSettings.RightPin1);
            var rightPin2 = _controller.Read(_trackerSettings.RightPin2);

            return new TrackerData()
            {
                LeftPin1 = leftPin1.ToString().Equals("high", StringComparison.OrdinalIgnoreCase),
                LeftPin2 = leftPin2.ToString().Equals("high", StringComparison.OrdinalIgnoreCase),
                RightPin1 = rightPin1.ToString().Equals("high", StringComparison.OrdinalIgnoreCase),
                RightPin2 = rightPin2.ToString().Equals("high", StringComparison.OrdinalIgnoreCase),
            };
        }
    }
}