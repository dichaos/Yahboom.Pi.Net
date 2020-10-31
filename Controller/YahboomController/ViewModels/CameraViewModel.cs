using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace YahboomController.ViewModels
{
    public class CameraViewModel : ClientViewModel
    {
        private IBitmap _latBitmap;
        
        public CameraViewModel(CancellationToken token,  Client c):base(c)
        {
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
            await Client.SetCameraHorizontal(angle);
        }
        
        public async Task SetVertical(int angle)
        {
            await Client.SetCameraVertical(angle);
        }
    }
}