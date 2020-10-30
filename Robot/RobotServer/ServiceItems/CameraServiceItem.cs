using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Robot.Devices;
using RobotControllerContract;

namespace RobotServer.ServiceItems
{
    public interface ICameraServiceItem
    {
        Reply CameraLeftRight(ServoRequest request);
        Reply CameraUpDown(ServoRequest request);
        Task VideoStream(IServerStreamWriter<VideoData> responseStream, CancellationToken token);
    }

    public class CameraServiceItem : ICameraServiceItem
    {
        public readonly ICamera _camera;
        
        public CameraServiceItem(ICamera camera)
        {
            _camera = camera;
        }
        
        public Reply CameraLeftRight(ServoRequest request)
        {
            try
            {
                _camera.SetHorizontalRadiance(request.Degree);
                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Reply() {Success = false};
            }
        }
        
        public Reply CameraUpDown(ServoRequest request)
        {
            try
            {
                _camera.SetVerticalRadiance(request.Degree);
                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Reply() {Success = false};
            }
        }
        
        public async Task VideoStream(IServerStreamWriter<VideoData> responseStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
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