using System;
using System.Device.Gpio;
using Robot.PWM;

namespace Robot.Devices
{
    public class DCMotor : IDisposable
    {
        private readonly GpioController _gpioController;
        private readonly int _pin1;
        private readonly int _pin2;
        private readonly PWMWiringPi _pwmWiringPi;

        public DCMotor(int speedPin, int pin1, int pin2, GpioController gpioController)
        {
            _pin1 = pin1;
            _pin2 = pin2;
            _gpioController = gpioController;

            _pwmWiringPi = new PWMWiringPi(speedPin);

            gpioController.OpenPin(_pin1, PinMode.Output);
            gpioController.Write(_pin1, PinValue.Low);

            gpioController.OpenPin(_pin2, PinMode.Output);
            gpioController.Write(_pin2, PinValue.Low);
        }

        public void Dispose()
        {
            Stop();
            _pwmWiringPi.Dispose();
        }

        public void SetSpeed(int speed)
        {
            _pwmWiringPi.SetDutyCycle(speed);
        }

        public void Backwards()
        {
            _gpioController.Write(_pin1, PinValue.Low);
            _gpioController.Write(_pin2, PinValue.High);
        }

        public void Forwards()
        {
            _gpioController.Write(_pin1, PinValue.High);
            _gpioController.Write(_pin2, PinValue.Low);
        }

        public void Stop()
        {
            _gpioController.Write(_pin1, PinValue.Low);
            _gpioController.Write(_pin2, PinValue.Low);
        }
    }
}