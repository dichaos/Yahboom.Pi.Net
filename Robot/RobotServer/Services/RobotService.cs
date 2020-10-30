using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RobotServer.ServiceItems;
using RobotControllerContract;

namespace RobotServer.Services
{
    public class RobotService : RobotControllerContract.Robot.RobotBase
    {
        private readonly ILogger<RobotService> _logger;
        private readonly ICameraServiceItem _camera;
        private readonly IRGBServiceItem _led;
        private readonly IAudioServiceItem _microphone;
        private readonly IMovementServiceItem _movement;
        private readonly ITrackerServiceItem _tracker;
        private readonly IUltrasonicServiceItem _ultrasonic;
        private readonly IBuzzServiceItem _buzzer;

        public RobotService(ILogger<RobotService> logger, 
            IBuzzServiceItem buzzer, 
            ICameraServiceItem camera, 
            IRGBServiceItem led, 
            IAudioServiceItem microphone, 
            IMovementServiceItem movement, 
            ITrackerServiceItem tracker,
            IUltrasonicServiceItem ultrasonic)
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
            var t = new Task<Reply>(() => {return _movement.Movement(request); });
            return await t;
        }

        public override async Task<Reply> Buzz(BuzzValue request, ServerCallContext context)
        {
            var t = new Task<Reply>(() => {return _buzzer.Buzz(request); });
            return await t;
        }
        
        public override async Task<Reply> CameraLeftRight(ServoRequest request, ServerCallContext context)
        {
            var t = new Task<Reply>(() => {return _camera.CameraLeftRight(request); });
            return await t;
        }
        
        public override async Task<Reply> CameraUpDown(ServoRequest request, ServerCallContext context)
        {
            var t = new Task<Reply>(() => {return _camera.CameraUpDown(request); });
            return await t;
        }
        
        public override async Task VideoStream(Empty request, IServerStreamWriter<VideoData> responseStream, ServerCallContext context)
        {
            await _camera.VideoStream(responseStream, context.CancellationToken);
        }
        
        public override async Task<Reply> UltrasonicLeftRight(ServoRequest request, ServerCallContext context)
        {
            var t = new Task<Reply>(() => {return _ultrasonic.UltrasonicLeftRight(request); });
            return await t;
        }
        
        public override async Task UltrasonicStream(Empty request, IServerStreamWriter<UltrasonicData> responseStream, ServerCallContext context)
        {
            await _ultrasonic.UltrasonicStream(responseStream, context.CancellationToken);
        }
        
        public override async Task<Reply> LED(LEDValue request, ServerCallContext context)
        {
            var t = new Task<Reply>(() => {return _led.LED(request); });
            return await t;
        }

        public override async Task AudioStream(Empty request, IServerStreamWriter<AudioData> responseStream, ServerCallContext context)
        {
            await _microphone.AudioStream(responseStream, context.CancellationToken);
        }

        public override async Task TrackerStream(Empty request, IServerStreamWriter<RobotControllerContract.TrackerData> responseStream, ServerCallContext context)
        {
            await _tracker.TrackerStream(responseStream, context.CancellationToken);
        }
    }
}