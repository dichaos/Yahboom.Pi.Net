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
        Task TrackerStream(IServerStreamWriter<TrackerData> responseStream, CancellationToken token);
    }

    public class TrackerServiceItem : ServiceItemBase, ITrackerServiceItem
    {
        private readonly ITracker _tracker;

        public TrackerServiceItem(ILogger<RobotService> logger, ITracker tracker) : base(logger)
        {
            _tracker = tracker;
        }

        public async Task TrackerStream(IServerStreamWriter<TrackerData> responseStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var data = _tracker.ReadValue();
                    await responseStream.WriteAsync(new TrackerData
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

                Task.Delay(TimeSpan.FromMilliseconds(1000), token).Wait(token);
            }
        }
    }
}