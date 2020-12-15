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

        public UltrasonicServiceItem(ILogger<RobotService> logger, IUltrasonic ultrasonic) : base(logger)
        {
            _ultrasonic = ultrasonic;
        }

        public Reply UltrasonicLeftRight(ServoRequest request)
        {
            try
            {
                _ultrasonic.SetRadiance(request.Degree);
                return new Reply {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error reading ultrasonic value");
                return new Reply {Success = false};
            }
        }

        public async Task UltrasonicStream(IServerStreamWriter<UltrasonicData> responseStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var data = new UltrasonicData
                    {
                        Value = _ultrasonic.ReadValue()
                    };

                    try
                    {
                        await responseStream.WriteAsync(data);
                    }
                    catch (InvalidOperationException ex)
                    {
                        if (!ex.Message.Contains("Cannot write message after request is complete"))
                            throw;
                    }
                    catch (OperationCanceledException)
                    {
                    }

                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex, "Error reading ultrasonic");
                }

                Task.Delay(TimeSpan.FromMilliseconds(1000), token).Wait(token);
            }
        }
    }
}