using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace Robot.Devices
{
    public interface IServo : IDisposable
    {
        void SetDutyCycle(int degree);
    }

    public class Servo : IServo
    {
        private readonly PWM.PWM _pwm;

        public Servo(int pin, GpioController gpioController)
        {
            _pwm = new PWM.PWM(pin, 100);
        }

        public void SetDutyCycle(int degree)
        {
            Console.WriteLine("Setting servo to " + degree);
            _pwm.SetDutyCycle(degree);
        }

        public void Dispose()
        {
            _pwm.Dispose();
        }
    }
}