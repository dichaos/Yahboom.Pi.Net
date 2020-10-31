using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading.Tasks;
using Iot.Device.Hcsr04;
using Robot.Configs;

namespace Robot.Devices
{
    public interface IUltrasonic : IDisposable
    {
        double ReadValue();
        void SetRadiance(int degree);
        void Forward();
        int GetRadiance();
    }

    public class Ultrasonic : IUltrasonic
    {
        private readonly Hcsr04 _sensor;
        private readonly Servo _servo;
        private readonly UltrasonicSettings _settings;

        public Ultrasonic(UltrasonicSettings ultraSettings, GpioController gpioGpioController)
        {
            
            _settings = ultraSettings;
            
            _sensor = new Hcsr04(gpioGpioController, _settings.TrigPin, _settings.EchoPin);
            _servo = new Servo(ultraSettings.Servo, gpioGpioController);
        }

        private double _lastValue;

        public double ReadValue()
        {
            try
            {
                _lastValue =  _sensor.Distance;
            }
            catch (InvalidOperationException)
            {
                
            }

            return _lastValue;
        }

        public void SetRadiance(int degree)
        {
            _servo.SetRadiance(degree);
        }

        public void Forward()
        {
            _servo.SetRadiance(180);
        }

        public int GetRadiance()
        {
            return _servo.GetAngle();
        }

        public void Dispose()
        {
            _servo.Dispose();
        }
    }
}