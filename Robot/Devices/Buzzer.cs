using System;
using System.Device.Gpio;

namespace Robot.Devices
{
    public class Buzzer
    {
        private GpioController _controller;
        private int  _buzzerPin;
        
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
        }

        public void Stop()
        {
            _controller.Write(_buzzerPin, PinValue.High);    
        }
    }
}