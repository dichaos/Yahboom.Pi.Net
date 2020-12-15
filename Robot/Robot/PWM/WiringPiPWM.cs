using System;

namespace Robot.PWM
{
    public class PWMWiringPi : IDisposable
    {
        private readonly int _pin;

        static PWMWiringPi()
        {
            WiringPi.wiringPiSetupGpio();
        }

        public PWMWiringPi(int pin, int range = 100)
        {
            _pin = pin;
            WiringPi.SoftPwmCreate(_pin, 0, range);
        }

        public void Dispose()
        {
            WiringPi.SoftPwmStop(_pin);
        }

        public void Stop()
        {
            WiringPi.SoftPwmStop(_pin);
        }

        public void SetDutyCycle(int pulse)
        {
            WiringPi.SoftPwmWrite(_pin, pulse);
        }
    }
}