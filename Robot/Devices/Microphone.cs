
using System;
using System.Linq;
using OpenAL;
using Robot.Configs;
using OpenTK;
using OpenTK.Audio.OpenAL;
using OpenTK.OpenAL;
using OpenTK.Core;

namespace Robot.Devices
{
    public class Microphone : IDisposable
    {
        private readonly ALCaptureDevice _captureDevice;
        private readonly AudioSettings _settings;
        public Microphone(AudioSettings settings)
        {
            _settings = settings;
            Console.WriteLine("Currently using device " + _settings.DeviceIndex+" if you want to use a different device please edit config file");
            
            var devices = ALC.GetStringList(GetEnumerationStringList.CaptureDeviceSpecifier);
            
            var d = Alc.CaptureOpenDevice(devices.ToList()[settings.DeviceIndex], settings.SampleRate, (int)ALFormat.Stereo16, settings.Chunk);
            _captureDevice = new ALCaptureDevice(d);
            ALC.CaptureStart(_captureDevice);
        }

        public static void PrintRecorders()
        {
            var devices = ALC.GetStringList(GetEnumerationStringList.CaptureDeviceSpecifier).ToList();
            
            Console.WriteLine("--- Available audio capture devices ---");
            for (int i = 0; i < devices.Count(); i++)
            {
                Console.WriteLine(i+" - "+devices[i]);
            }
            Console.WriteLine("---------------------------------------");
        }

        public short[] Read()
        {
            var current = 0;
            var recording = new short[_settings.SampleRate * 4];
            
            while (current < recording.Length)
            {
                var samplesAvailable = ALC.GetAvailableSamples(_captureDevice);
                if (samplesAvailable > 512)
                {
                    var samplesToRead = Math.Min(samplesAvailable, recording.Length - current);
                    ALC.CaptureSamples(_captureDevice, ref recording[current], samplesToRead);
                    current += samplesToRead;
                }
            }
            return recording;
        }

        public void Dispose()
        {
            try { ALC.CaptureStop(_captureDevice); }catch {}
        }
    }
}