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
    public interface ITrackerServiceItem
    {
        Task TrackerStream(IServerStreamWriter<RobotControllerContract.TrackerData> responseStream, CancellationToken token);
    }

    public class TrackerServiceItem : ServiceItemBase, ITrackerServiceItem
    {
        private readonly Robot.Devices.ITracker _tracker;
        
        public TrackerServiceItem(ILogger<RobotService> logger, Robot.Devices.ITracker tracker):base(logger)
        {
            _tracker = tracker;
        }
        
        public async Task TrackerStream(IServerStreamWriter<RobotControllerContract.TrackerData> responseStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var data = _tracker.ReadValue();
                    await responseStream.WriteAsync(new RobotControllerContract.TrackerData()
                    {
                        LeftPin1 = data.LeftPin1,
                        LeftPin2 = data.LeftPin2,
                        RightPin1 = data.RightPin1,
                        RightPin2 = data.RightPin2
                    });
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex, "Error reading tracker values");
                }

                Task.Delay(1000).Wait();
            }
        }
    }
}