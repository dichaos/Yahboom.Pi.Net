using System;
using System.Device.Gpio;
using Robot.Configs;
using Robot.Devices;

namespace Robot
{
    public class Robot : IDisposable
    {
        public readonly Buzzer Buzzer;
        public readonly Camera Camera;
        public readonly LED LED;
        public readonly Microphone Microphone;
        public readonly Movement Movement;
        public readonly Tracker Tracker;
        public readonly Ultrasonic Ultrasonic;

        private readonly GpioController _gpioController;
        
        public Robot()
        {
            var settings = RobotSettings.GetConfig("config.json");

            _gpioController = new GpioController(PinNumberingScheme.Logical);

            Buzzer = new Buzzer(settings.Buzzer, _gpioController);
            Console.WriteLine("Created buzzer");
            
            LED = new LED(settings.LEDSettings, _gpioController);
            Console.WriteLine("Created LED");
            
            Tracker = new Tracker(settings.TrackerSettings, _gpioController);
            Console.WriteLine("Created Tracker");
            
            Movement = new Movement(settings.MovementSettings, _gpioController);
            Movement.SetSpeed(0.5);
            Console.WriteLine("Created movement");
            
            Ultrasonic = new Ultrasonic(settings.UltrasonicSettings, _gpioController);
            Console.WriteLine("Created Ultrasonic");
            
            Camera = new Camera(settings.CameraSettings, _gpioController);
            Console.WriteLine("Created camera");
            
            Microphone.PrintRecorders();
            Microphone = new Microphone(settings.AudioSettings);
            Console.WriteLine("Created audio");

            Console.WriteLine("Robot GPIOs ready");
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing LED");
            try{ LED.SetRGB(0, 0, 0); }catch{}
            Console.WriteLine("Disposing Ultrasonic");
            try{ Ultrasonic.Dispose(); }catch{}
            Console.WriteLine("Disposing Camera");
            try{ Camera.Dispose(); }catch{}
            Console.WriteLine("Disposing Microphone");
            try{ Microphone.Dispose(); }catch{}
            Console.WriteLine("Disposing Movement");
            try{ Movement.Dispose(); }catch{}
            _gpioController.Dispose();
        }
    }
}