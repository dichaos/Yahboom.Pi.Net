using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace Robot.Devices
{
    public interface IServo
    {
        void SetDutyCycle(int degree);
    }

    public class Servo : IServo
    {
        private readonly PWM.PWM _pwm;

        public Servo(int pin, GpioController gpioController)
        {
            _pwm = new PWM.PWM(gpioController, pin, 500, 2500);
        }

        public void SetDutyCycle(int value)
        {
            var c = new CancellationTokenSource();
            Task.Factory.StartNew(() => _pwm.SetDutyCycle(value, c.Token));
            c.CancelAfter(TimeSpan.FromMilliseconds(300));
        }
    }
}