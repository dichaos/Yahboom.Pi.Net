using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Robot;

namespace ControllerClient
{
    public class Client : IDisposable
    {
        private readonly GrpcChannel _channel;
        private readonly Robot.Robot.RobotClient _client;
        
        public Client(string url)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            _channel = GrpcChannel.ForAddress(new Uri(url));  
            _client = new Robot.Robot.RobotClient(_channel);
        }
        
        public async Task Buzz(bool onOff)
        {
            await _client.BuzzAsync(new BuzzValue()
            {
                OnOff = onOff
            });
        }

        public async Task SetUltrasonic(int degree)
        {
            await _client.UltrasonicLeftRightAsync(new ServoRequest()
            {
                Degree = degree
            });
        }

        public async Task SetCameraHorizontal(int degree)
        {
            await _client.CameraLeftRightAsync(new ServoRequest()
            {
                Degree = degree
            });
        }

        public async Task SetCameraVertical(int degree)
        {
            await _client.CameraUpDownAsync(new ServoRequest()
            {
                Degree = degree
            });
        }

        public async Task SetMovement(MovementRequest.Types.Direction d)
        {
            await _client.MovementAsync(new MovementRequest()
            {
                MovementDirection = d
            });
        }

        public async Task SetMovementSpeed(int speed)
        {
            await _client.MovementAsync(new MovementRequest()
            {
                MovementDirection = MovementRequest.Types.Direction.Speed,
                Speed = speed
            });
        }

        public async Task<State> GetRobotState()
        {
            return await _client.GetCurrentStateAsync(new Empty());
        }

        public async Task SetLED(int Red, int Green, int Blue)
        {
            var response = await _client.LEDAsync(new LEDValue()
            {
                Red = Red,
                Green = Green,
                Blue = Blue
            });
            
            
        }
        private Task GetVideo(CancellationToken token, Action<byte[]> processor)
        {
            return new Task(async () =>
            {
                using var videoStream = _client.VideoStream(new Empty(), null, null, token);
                
                try
                {
                    await foreach (var item in  videoStream.ResponseStream.ReadAllAsync(token))
                    {
                        var bytes = new byte[item.Image.Length];
                        item.Image.CopyTo(bytes, 0);
                        processor(bytes);
                    }
                }
                catch(RpcException exc)
                {
                    Console.WriteLine(exc.Message);
                }
            });
        }

        public Task GetAudio(CancellationToken token, Action<short[]> processor)
        {
            return new Task(async () =>
            {
                using var audioStream = _client.AudioStream(new Empty(), null, null, token);
                
                try
                {
                    await foreach (var item in  audioStream.ResponseStream.ReadAllAsync(token))
                    {
                        processor(item.Data.ToArray().Select(x => (short)x).ToArray());
                    }
                }
                catch(RpcException exc)
                {
                    Console.WriteLine(exc.Message);
                }
            });
        }

        public Task GetUltrasonic(CancellationToken token, Action<double> processor)
        {
            return new Task(async () =>
            {
                using var ultrasonicStream = _client.UltrasonicStream(new Empty(), null, null, token);

                try
                {
                    await foreach (var item in ultrasonicStream.ResponseStream.ReadAllAsync(token))
                    {
                        processor(item.Value);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        private Task GetTrackerValues(CancellationToken token, Action<TrackerData> processor)
        {
            return new Task(async () =>
            {
                using var trackerStream = _client.TrackerStream(new Empty(), null, null, token);

                try
                {
                    await foreach (var item in trackerStream.ResponseStream.ReadAllAsync(token))
                    {
                        processor(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
        
        public void Dispose()
        {
            _channel.Dispose();
        }
    }
}