using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading.Tasks;
using Robot.Configs;

namespace Robot.Devices
{
    public class Ultrasonic : IDisposable
    {
        private readonly GpioController _gpioController;
        private readonly Servo _servo;
        private readonly UltrasonicSettings _settings;

        public Ultrasonic(UltrasonicSettings ultraSettings, GpioController gpioGpioController)
        {
            _settings = ultraSettings;
            
            _gpioController = gpioGpioController;
            _gpioController.OpenPin(_settings.EchoPin, PinMode.Input);
            _gpioController.OpenPin(_settings.TrigPin, PinMode.Output);

            _servo = new Servo(ultraSettings.Servo, gpioGpioController);
        }

        public double ReadValue()
        {
            _gpioController.Write(_settings.TrigPin, PinValue.High);
            Task.Delay(1).Wait();
            _gpioController.Write(_settings.TrigPin, PinValue.Low);

            while (_gpioController.Read(_settings.EchoPin) == PinValue.Low)
            {
            }

            var delay = new Stopwatch();
            delay.Start();
                
            while(_gpioController.Read(_settings.EchoPin) == PinValue.High)
                delay.Stop();
            
            //multiply with the sonic speed (34300 cm/s)
            //and divide by 2, because there and back

            var distance = (delay.Elapsed.TotalSeconds * 3400) / 2;

            return distance;
        }

        public void SetRadiance(int degree)
        {
            _servo.SetRadiance(degree);
        }

        public void Forward()
        {
            _servo.SetRadiance(180);
        }
        
        public void Dispose()
        {
            _servo.Dispose();
        }
    }
}