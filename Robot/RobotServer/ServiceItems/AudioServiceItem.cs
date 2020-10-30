using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Robot.Devices;
using RobotControllerContract;

namespace RobotServer.ServiceItems
{
    public interface IAudioServiceItem
    {
        Task AudioStream(IServerStreamWriter<AudioData> responseStream, CancellationToken token);
    }

    public class AudioServiceItem : IAudioServiceItem
    {
        private readonly IMicrophone _microphone;
        
        public AudioServiceItem(IMicrophone microphone)
        {
            _microphone = microphone;
        }
        
        public async Task AudioStream(IServerStreamWriter<AudioData> responseStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await responseStream.WriteAsync(new AudioData()
                {
                    Data = {_microphone.Read().Select(x => (int)x)}
                });
            }
        }
    }
}