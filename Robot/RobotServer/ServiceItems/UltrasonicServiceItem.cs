using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Robot.Devices;
using RobotControllerContract;
using RobotServer.Services;

namespace RobotServer.ServiceItems
{
    public interface IUltrasonicServiceItem
    {
        Reply UltrasonicLeftRight(ServoRequest request);
        Task UltrasonicStream(IServerStreamWriter<UltrasonicData> responseStream, CancellationToken token);
    }

    public class UltrasonicServiceItem : ServiceItemBase, IUltrasonicServiceItem
    {
        private readonly IUltrasonic _ultrasonic;

        public UltrasonicServiceItem(ILogger<RobotService> logger, IUltrasonic ultrasonic):base(logger)
        {
            _ultrasonic = ultrasonic;
        }
        
        public Reply UltrasonicLeftRight(ServoRequest request)
        {
            try
            {
                _ultrasonic.SetRadiance(request.Degree);
                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error reading ultrasonic value");
                return new Reply() {Success = false};
            }
        }
        
        public async Task UltrasonicStream(IServerStreamWriter<UltrasonicData> responseStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new UltrasonicData()
                {
                    Value = _ultrasonic.ReadValue() 
                });

                Task.Delay(1000).Wait();
            }
        }
    }
}