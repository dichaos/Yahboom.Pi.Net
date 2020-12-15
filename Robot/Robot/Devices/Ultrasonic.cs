using System;
using System.Device.Gpio;
using Iot.Device.Hcsr04;
using Robot.Configs;

namespace Robot.Devices
{
    public interface IUltrasonic
    {
        double ReadValue();
        void SetRadiance(int degree);
    }

    public class Ultrasonic : IUltrasonic
    {
        private readonly Hcsr04 _sensor;
        private readonly Servo _servo;
        private readonly UltrasonicSettings _settings;

        private double _lastValue;

        public Ultrasonic(UltrasonicSettings ultraSettings, GpioController gpioGpioController)
        {
            _settings = ultraSettings;

            _sensor = new Hcsr04(gpioGpioController, _settings.TrigPin, _settings.EchoPin);
            _servo = new Servo(ultraSettings.Servo, gpioGpioController);
        }

        public double ReadValue()
        {
            try
            {
                _lastValue = _sensor.Distance.Centimeters;
            }
            catch (InvalidOperationException)
            {
            }

            return _lastValue;
        }

        public void SetRadiance(int degree)
        {
            _servo.SetDutyCycle(degree);
        }
    }
}