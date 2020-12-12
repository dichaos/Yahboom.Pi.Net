using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YahboomController.ViewModels;

namespace YahboomController.Views
{
    public class Camera : UserControl
    {
        private readonly Slider _horizontalSlider;
        private readonly Slider _verticalSlider;
        private readonly Button _button;
        
        public Camera()
        {
            InitializeComponent();
            
            _horizontalSlider = this.FindControl<Slider>("HorizontalSlider");
            _verticalSlider = this.FindControl<Slider>("VerticalSlider");
            _button = this.FindControl<Button>("CenterButton");
            
            _horizontalSlider.PointerCaptureLost += async (sender, args) =>
            {
                await ChangeHorizontal();
            };
            
            _verticalSlider.PointerCaptureLost += async (sender, args) =>
            {
                await ChangeVertical();
            };

            _button.Click += async (sender, args) =>
            {
                await Center();
            };
        }

        private async Task Center()
        {
            _horizontalSlider.Value = 90;
            _verticalSlider.Value = 90;
            
            await ChangeHorizontal();
            await ChangeVertical();
        }

        private async Task ChangeHorizontal()
        {
            var client = ((CameraViewModel) this.DataContext)?.Client;
            if (client != null) await client.SetCameraHorizontal((int)_horizontalSlider.Value);
        }

        private async Task ChangeVertical()
        {
            var client = ((CameraViewModel) this.DataContext)?.Client;
            if (client != null) await client.SetCameraVertical((int) _verticalSlider.Value);
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}