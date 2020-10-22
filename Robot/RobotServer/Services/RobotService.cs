using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Robot;
using Robot.Devices;

namespace RobotServer
{
    public class RobotService : Robot.Robot.RobotBase
    {
        private readonly ILogger<RobotService> _logger;
        private readonly ICamera _camera;
        private readonly ILED _led;
        private readonly IMicrophone _microphone;
        private readonly IMovement _movement;
        private readonly ITracker _tracker;
        private readonly IUltrasonic _ultrasonic;
        private readonly IBuzzer _buzzer;

        public RobotService(ILogger<RobotService> logger, 
            IBuzzer buzzer, 
            ICamera camera, 
            ILED led, 
            IMicrophone microphone, 
            IMovement movement, 
            ITracker tracker,
            IUltrasonic ultrasonic)
        {
            _logger = logger;
            _camera = camera;
            _led = led;
            _microphone = microphone;
            _tracker = tracker;
            _movement = movement;
            _ultrasonic = ultrasonic;
            _buzzer = buzzer;
        }
        
        public override async Task<Reply> Movement(MovementRequest request, ServerCallContext context)
        {
            try
            {
                switch (request.MovementDirection)
                {
                    case MovementRequest.Types.Direction.Forwards:
                        _movement.Forward();
                        break;
                    case MovementRequest.Types.Direction.Backwards:
                        _movement.Backward();
                        break;
                    case MovementRequest.Types.Direction.Left:
                        _movement.TurnLeft();
                        break;
                    case MovementRequest.Types.Direction.Right:
                        _movement.TurnRight();
                        break;
                    case MovementRequest.Types.Direction.Stop:
                        _movement.Stop();
                        break;
                    case MovementRequest.Types.Direction.Speed:
                        _movement.SetSpeed(request.Speed);
                        break;
                }

                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,"Error in movement",ex);
                return new Reply() {Success = false};
            }
        }

        public override async Task<Reply> Buzz(BuzzValue request, ServerCallContext context)
        {
            try
            {
                if(request.OnOff)
                    _buzzer.Start();
                else
                {
                    _buzzer.Stop();
                }

                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,"Error in movement",ex);
                return new Reply() {Success = false};
            }
        }
        
        public override async Task<Reply> CameraLeftRight(ServoRequest request, ServerCallContext context)
        {
            try
            {
                _camera.SetHorizontalRadiance(request.Degree);
                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,"Error in movement",ex);
                return new Reply() {Success = false};
            }
        }
        
        public override async Task<Reply> CameraUpDown(ServoRequest request, ServerCallContext context)
        {
            try
            {
                _camera.SetVerticalRadiance(request.Degree);
                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,"Error in movement",ex);
                return new Reply() {Success = false};
            }
        }
        
        public override async Task<Reply> LED(LEDValue request, ServerCallContext context)
        {
            try
            {
                _led.SetRGB(request.Red, request.Blue, request.Green);
                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,"Error in movement",ex);
                return new Reply() {Success = false};
            }
        }

        public override async Task<Reply> UltrasonicLeftRight(ServoRequest request, ServerCallContext context)
        {
            try
            {
                _ultrasonic.SetRadiance(request.Degree);
                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error,"Error in movement",ex);
                return new Reply() {Success = false};
            }
        }

        public override async Task<State> GetCurrentState(Empty request, ServerCallContext context)
        {
            return new State()
            {
                Buzz = _buzzer.GetOnOff(),
                CameraHorizontal = _camera.GetHorizontal(),
                CameraVertical = _camera.GetVertical(),
                MovementSpeed = _movement.GetSpeed(),
                UltrasonicServo = _ultrasonic.GetRadiance(),
                LED = new LEDValue()
                {
                    Red = _led.GetRed(),
                    Green = _led.GetGreen(),
                    Blue = _led.GetBlue()
                }
            };
        }

        public override async Task AudioStream(Empty request, IServerStreamWriter<AudioData> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new AudioData()
                {
                    Data = {_microphone.Read().Select(x => (int)x)}
                });
            }
        }

        public override async Task TrackerStream(Empty request, IServerStreamWriter<TrackerData> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                var data = _tracker.ReadValue();
                await responseStream.WriteAsync(new TrackerData()
                {
                    LeftPin1 = data.LeftPin1,
                    LeftPin2 = data.LeftPin2,
                    RightPin1 = data.RightPin1,
                    RightPin2 = data.RightPin2
                });

                Task.Delay(1000).Wait();
            }
        }

        public override async Task UltrasonicStream(Empty request, IServerStreamWriter<UltrasonicData> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new UltrasonicData()
                {
                    Value = _ultrasonic.ReadValue() 
                });

                Task.Delay(1000).Wait();
            }
        }

        public override async Task VideoStream(Empty request, IServerStreamWriter<VideoData> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new VideoData()
                {
                    Image = ByteString.CopyFrom(_camera.ReadImage())
                });

                Task.Delay(16).Wait();
            }
        }
    }
}