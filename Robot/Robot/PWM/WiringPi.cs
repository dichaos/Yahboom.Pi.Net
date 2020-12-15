using System.Runtime.InteropServices;

namespace Robot.PWM
{
    public static class WiringPi
    {
        private const string WiringPiLibrary = "/usr/lib/libwiringPi.so.2.50";

        [DllImport(WiringPiLibrary, EntryPoint = "wiringPiSetupGpio", SetLastError = true)]
        public static extern int wiringPiSetupGpio();

        [DllImport(WiringPiLibrary, EntryPoint = "softPwmCreate", SetLastError = true)]
        public static extern int SoftPwmCreate(int pin, int initialValue, int pwmRange);

        [DllImport(WiringPiLibrary, EntryPoint = "softPwmWrite", SetLastError = true)]
        public static extern void SoftPwmWrite(int pin, int value);

        [DllImport(WiringPiLibrary, EntryPoint = "softPwmStop", SetLastError = true)]
        public static extern void SoftPwmStop(int pin);

        [DllImport(WiringPiLibrary, EntryPoint = "wiringPiSetupGpio", SetLastError = true)]
        public static extern void softServoWrite(int servoPin, int value);
    }
}