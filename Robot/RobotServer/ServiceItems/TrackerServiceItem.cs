using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using RobotControllerContract;

namespace RobotServer.ServiceItems
{
    public interface ITrackerServiceItem
    {
        Task TrackerStream(IServerStreamWriter<TrackerData> responseStream, CancellationToken token);
    }

    public class TrackerServiceItem : ITrackerServiceItem
    {
        private readonly Robot.Devices.ITracker _tracker;
        
        public TrackerServiceItem(Robot.Devices.ITracker tracker)
        {
            _tracker = tracker;
        }
        
        public async Task TrackerStream(IServerStreamWriter<TrackerData> responseStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
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
    }
}