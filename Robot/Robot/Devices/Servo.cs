using System;
using System.Device.Gpio;
using System.Threading.Tasks;

namespace Robot.Devices
{
    public interface IServo : IDisposable
    {
        void SetRadiance(int degree);
        void Forward();
    }

    public class Servo : IServo
    {
        private const long MaxPulseWidth = 2450; //microseconds
        private const long MinPulseWidth = 450; //microseconds

        private readonly PWM.PWM _pwm;
        
        public Servo(int pin, GpioController gpioController)
        {
            _pwm = new PWM.PWM(gpioController, pin, MinPulseWidth, MaxPulseWidth);
        }

        public void SetRadiance(int degree)
        {
            var dutyCycle = RadianceToDytyCycle(degree);
            _pwm.SetDutyCycle(dutyCycle);
            
            Task.Delay(1000).Wait();
            _pwm.Stop();
        }

        private long RadianceToDytyCycle(int angle)
        {
            return (( (MaxPulseWidth-MinPulseWidth) * angle) / 360) + MinPulseWidth;
        }

        public void Forward() => SetRadiance(90);

        public void Dispose()
        {
            _pwm.Stop();
        }
    }
}