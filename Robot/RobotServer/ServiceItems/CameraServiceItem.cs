using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Robot.Devices;
using RobotControllerContract;
using RobotServer.Services;

namespace RobotServer.ServiceItems
{
    public interface ICameraServiceItem
    {
        Reply CameraLeftRight(ServoRequest request);
        Reply CameraUpDown(ServoRequest request);
        Task VideoStream(IServerStreamWriter<VideoData> responseStream, CancellationToken token);
    }

    public class CameraServiceItem : ServiceItemBase, ICameraServiceItem
    {
        public readonly ICamera _camera;
        
        public CameraServiceItem(ILogger<RobotService> logger, ICamera camera):base(logger)
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
                _logger.Log(LogLevel.Error, ex, "Error turning camera horizontally");
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
                _logger.Log(LogLevel.Error, ex, "Error turning camera vertically");
                return new Reply() {Success = false};
            }
        }
        
        public async Task VideoStream(IServerStreamWriter<VideoData> responseStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    await responseStream.WriteAsync(new VideoData()
                    {
                        Image = ByteString.CopyFrom(_camera.ReadImage())
                    });
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex, "Error reading video");
                }

                Task.Delay(16).Wait();
            }
        }
    }
}