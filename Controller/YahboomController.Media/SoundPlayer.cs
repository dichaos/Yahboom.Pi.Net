using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenTK.Audio.OpenAL;
using OpenTK.Audio.OpenAL.Extensions.Creative.EFX;
using OpenTK.Audio.OpenAL.Extensions.EXT.Double;
using OpenTK.Audio.OpenAL.Extensions.EXT.Float32;
using OpenTK.Audio.OpenAL.Extensions.EXT.FloatFormat;

namespace YahboomController.Media
{
    public class SoundPlayer : IDisposable
    {
        private readonly ALDevice _device;
        private readonly ALContext _context;
        private readonly int _auxSlot;

        public SoundPlayer()
        {
            PrintDevices();
            
            var devices = ALC.GetStringList(GetEnumerationStringList.DeviceSpecifier);
            
            // Get the default device, then go though all devices and select the AL soft device if it exists.
            string deviceName = ALC.GetString(ALDevice.Null, AlcGetString.DefaultDeviceSpecifier);

            Console.WriteLine("Device Selected: " + deviceName);
            
            foreach (var d in devices)
            {
                if (d.Contains("OpenAL Soft"))
                {
                    deviceName = d;
                }
            }

            _device = ALC.OpenDevice(deviceName);
            PrintDeviceInfo(_device);
            
            var context = ALC.CreateContext(_device, (int[]) null);
            ALC.MakeContextCurrent(context);

            CheckALError("Create context and get device");

            var auxSlot = 0;
            if (EFX.IsExtensionPresent(_device))
            {
                EFX.GenEffect(out int effect);
                EFX.Effect(effect, EffectInteger.EffectType, (int) EffectType.Reverb);
                EFX.GenAuxiliaryEffectSlot(out _auxSlot);
                EFX.AuxiliaryEffectSlot(auxSlot, EffectSlotInteger.Effect, effect);
            }
        }

        public void Play(short[] sound_data)
        {
            if (sound_data == null || sound_data.Length == 0)
                return;
            
            AL.GenBuffer(out int alBuffer);
            AL.BufferData(alBuffer, ALFormat.Mono16, ref sound_data[0], sound_data.Length * 2, 44100);
            CheckALError("BufferData");

            AL.Listener(ALListenerf.Gain, 0.1f);
            CheckALError("Listener"); 
            AL.GenSource(out int alSource);
            CheckALError("GenSource"); 
            AL.Source(alSource, ALSourcef.Gain, 1f);
            CheckALError("Source-1"); 
            AL.Source(alSource, ALSourcei.Buffer, alBuffer);
            CheckALError("Source-2");
            
            if (EFX.IsExtensionPresent(_device))
            {
                EFX.Source(alSource, EFXSourceInteger3.AuxiliarySendFilter, _auxSlot, 0, 0);
                CheckALError("Source-3");
            }

            AL.SourcePlay(alSource);
            CheckALError("SourcePlay");

            
        }
    
        public static void CheckALError(string str)
        {
            ALError error = AL.GetError();
            
            if (error != ALError.NoError)
            {
                Console.WriteLine($"ALError at '{str}': {AL.GetErrorString(error)} ");
            }
        }

        private void PrintDevices()
        {
            var devices = ALC.GetStringList(GetEnumerationStringList.DeviceSpecifier);

            Console.WriteLine("-------------- Devices found -------------- ");
            foreach (var d in devices)
            {
                Console.WriteLine(d);
            }
            Console.WriteLine("------------------------------------------- ");
        }

        private void PrintDeviceInfo(ALDevice device)
        {
            
            ALC.GetInteger(device, AlcGetInteger.MajorVersion, 1, out int alcMajorVersion);
            ALC.GetInteger(device, AlcGetInteger.MinorVersion, 1, out int alcMinorVersion);
            string alcExts = ALC.GetString(device, AlcGetString.Extensions);

            var attrs = ALC.GetContextAttributes(device);
            Console.WriteLine($"Attributes: {attrs}");

            string exts = AL.Get(ALGetString.Extensions);
            string rend = AL.Get(ALGetString.Renderer);
            string vend = AL.Get(ALGetString.Vendor);
            string vers = AL.Get(ALGetString.Version);
            Console.WriteLine($"Vendor: {vend}, Version: {vers}, Renderer: {rend}, Extensions: {exts}, ALC Version: {alcMajorVersion}.{alcMinorVersion}, \nALC Extensions: {alcExts}");

        }

        public void Dispose()
        {
            //Marshal.FreeHGlobal(_context.Handle);
            //Marshal.FreeHGlobal(_device.Handle);
        }
        
    }
}