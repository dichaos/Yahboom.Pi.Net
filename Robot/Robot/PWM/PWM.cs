using System;
using System.Runtime.InteropServices;

namespace Robot.PWM
{
    public class PWM : IDisposable
    {
        private const string WiringPiLibrary = "/usr/lib/libwiringPi.so.2.50";

        [DllImport(WiringPiLibrary, EntryPoint = "wiringPiSetupGpio", SetLastError = true)]
        private static extern int wiringPiSetupGpio();
        
        [DllImport(WiringPiLibrary, EntryPoint = "softPwmCreate", SetLastError = true)]
        private static extern int SoftPwmCreate(int pin, int initialValue, int pwmRange);

        [DllImport(WiringPiLibrary, EntryPoint = "softPwmWrite", SetLastError = true)]
        private static extern void SoftPwmWrite(int pin, int value);

        [DllImport(WiringPiLibrary, EntryPoint = "softPwmStop", SetLastError = true)]
        private static extern void SoftPwmStop(int pin);

        private readonly int _pin;

        static PWM()
        {
            wiringPiSetupGpio();
        }
        
        public PWM(int pin, int range = 100)
        {
            _pin = pin;
            SoftPwmCreate(_pin, 0, range);
        }

        public void Dispose()
        {
            SoftPwmStop(_pin);
        }

        public void Stop()
        {
            SoftPwmStop(_pin);
        }

        public void SetDutyCycle(int pulse)
        {
            SoftPwmWrite(_pin, pulse);
        }
    }
}