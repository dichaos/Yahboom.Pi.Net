using System;
using System.Device.Gpio;

namespace Robot.Devices
{
    public interface IBuzzer
    {
        void Start();
        void Stop();
        bool GetOnOff();
    }

    public class Buzzer : IBuzzer
    {
        private GpioController _controller;
        private int  _buzzerPin;
        private bool _onOff;
        
        public Buzzer(int buzzer, GpioController controller)
        {
            _buzzerPin = buzzer;
            _controller = controller;
            _controller.OpenPin(_buzzerPin, PinMode.Output);
            _controller.Write(_buzzerPin, PinValue.High);
            
        }

        public void Start()
        {
            _controller.Write(_buzzerPin, PinValue.Low);
            _onOff = true;
        }

        public void Stop()
        {
            _controller.Write(_buzzerPin, PinValue.High);
            _onOff = false;
        }

        public bool GetOnOff()
        {
            return _onOff;
        }
    }
}