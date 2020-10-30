using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Robot.Devices;
using RobotControllerContract;

namespace RobotServer.ServiceItems
{
    public interface IUltrasonicServiceItem
    {
        Reply UltrasonicLeftRight(ServoRequest request);
        Task UltrasonicStream(IServerStreamWriter<UltrasonicData> responseStream, CancellationToken token);
    }

    public class UltrasonicServiceItem : IUltrasonicServiceItem
    {
        private readonly IUltrasonic _ultrasonic;

        public UltrasonicServiceItem(IUltrasonic ultrasonic)
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
                Console.WriteLine(ex);
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