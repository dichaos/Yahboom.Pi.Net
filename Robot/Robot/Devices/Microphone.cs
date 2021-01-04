using System;
using System.Linq;
using System.Threading;
using OpenAL;
using OpenTK.Audio.OpenAL;
using Robot.Configs;

namespace Robot.Devices
{
    public interface IMicrophone : IDisposable
    {
        short[] Read();
    }

    public class Microphone : IMicrophone
    {
        private ALCaptureDevice _captureDevice;
        private readonly AudioSettings _settings;

        public Microphone(AudioSettings settings)
        {
            _settings = settings;

            InitialiseALC();
            PrintRecorders();
        }

        private void InitialiseALC()
        {
            var devices = ALC.GetStringList(GetEnumerationStringList.CaptureDeviceSpecifier);
            var d = Alc.CaptureOpenDevice(devices.ToList()[_settings.DeviceIndex], _settings.SampleRate, (int) ALFormat.Mono16, _settings.Chunk);
            _captureDevice = new ALCaptureDevice(d);
            ALC.CaptureStart(_captureDevice);
        }

        public short[] Read()
        {
            var current = 0;
            var recording = new short[_settings.SampleRate * 4];

            while (current < recording.Length)
            {
                var samplesAvailable = ALC.GetAvailableSamples(_captureDevice);

                if (samplesAvailable == 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(5));
                    continue;
                }

                var r = new short[samplesAvailable];
                ALC.CaptureSamples(_captureDevice, ref r[0], samplesAvailable);
                return r;
            }

            return recording;
        }

        public void Dispose()
        {
            try
            {
                ALC.CaptureStop(_captureDevice);
            }
            catch(Exception ex)
            {
                Console.Write("ERROR: Error disposing ALC "+ex);
            }
        }

        private void PrintRecorders()
        {
            var devices = ALC.GetStringList(GetEnumerationStringList.CaptureDeviceSpecifier).ToList();

            Console.WriteLine("--- Available audio capture devices ---");
            for (var i = 0; i < devices.Count(); i++) Console.WriteLine(i + " - " + devices[i]);
            Console.WriteLine("---------------------------------------");
            
            Console.WriteLine("Currently using device : ("+_settings.DeviceIndex+")" + devices[_settings.DeviceIndex] + " if you want to use a different device please edit config file");
        }
    }
}