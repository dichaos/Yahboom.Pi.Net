using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace YahboomController.ViewModels
{
    public class CameraViewModel : ViewModelBase
    {
        private Client _client;
        private IBitmap _latBitmap;
        
        public CameraViewModel(CancellationToken token,  Client c)
        {
            _client = c;
            
            var task = c.GetVideo(token, Process);
            task.Start();
        }

        private void Process(byte[] image)
        {
            using (var ms = new MemoryStream(image))
            {
                Image = new Bitmap(ms);
            }
        }
        
        public IBitmap Image
        {
            get => _latBitmap;
            set => this.RaiseAndSetIfChanged(ref _latBitmap, value);
        }

        public async Task SetHorizontal(int angle)
        {
            await _client.SetCameraHorizontal(angle);
        }
        
        public async Task SetVertical(int angle)
        {
            await _client.SetCameraVertical(angle);
        }
    }
}