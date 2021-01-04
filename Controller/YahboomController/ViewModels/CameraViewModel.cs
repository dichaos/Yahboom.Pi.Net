using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ReactiveUI;
using YahboomController.Media;

namespace YahboomController.ViewModels
{
    public class CameraViewModel : ClientViewModel, IDisposable
    {
        private IBitmap _latBitmap;
        private SoundPlayer _player;

        private static object _lock = new object();
        
        public CameraViewModel(CancellationToken token, Client c) : base(c)
        {
            _player = new SoundPlayer(); 
            
            var videoTask = c.GetVideo(token, ProcessVideo);
            videoTask.Start();

            var audioTask = c.GetAudio(token, ProcessAudio);
            audioTask.Start();
        }

        public IBitmap Image
        {
            get => _latBitmap;
            set => this.RaiseAndSetIfChanged(ref _latBitmap, value);
        }

        private void ProcessVideo(byte[] image)
        {
            using var ms = new MemoryStream(image);
            Image = new Bitmap(ms);
        }

        private void ProcessAudio(short[] audio)
        {
            try
            {
                _player.Play(audio);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SetHorizontal(int angle)
        {
            await Client.SetCameraHorizontal(angle);
        }

        public async Task SetVertical(int angle)
        {
            await Client.SetCameraVertical(angle);
        }

        public void Dispose()
        {
            _player.Dispose();
        }
    }
}