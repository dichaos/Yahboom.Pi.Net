using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Robot.Devices;
using RobotControllerContract;
using RobotServer.Services;

namespace RobotServer.ServiceItems
{
    public interface IAudioServiceItem
    {
        Task AudioStream(IServerStreamWriter<AudioData> responseStream, CancellationToken token);
    }

    public class AudioServiceItem : ServiceItemBase, IAudioServiceItem
    {
        private readonly IMicrophone _microphone;

        public AudioServiceItem(ILogger<RobotService> logger, IMicrophone microphone) : base(logger)
        {
            _microphone = microphone;
        }

        public async Task AudioStream(IServerStreamWriter<AudioData> responseStream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
                try
                {
                    await responseStream.WriteAsync(new AudioData
                    {
                        Data = {_microphone.Read().Select(x => (int) x)}
                    });
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex, "Error in audio stream");
                }
        }
    }
}