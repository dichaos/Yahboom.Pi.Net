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
            
            _horizontalSlider.PointerCaptureLost += (sender, args) =>
            {
                ChangeHorizontal();
            };
            
            _verticalSlider.PointerCaptureLost += (sender, args) =>
            {
                ChangeVertical();
            };

            _button.Click += (sender, args) =>
            {
                Center();
            };
        }

        private void Center()
        {
            _horizontalSlider.Value = 90;
            _verticalSlider.Value = 90;
            ChangeHorizontal();
            ChangeVertical();
        }

        private void ChangeHorizontal()
        {
            ((CameraViewModel) this.DataContext)?.Client.SetCameraHorizontal((int)_horizontalSlider.Value);
        }

        private void ChangeVertical()
        {
            ((CameraViewModel) this.DataContext)?.Client.SetCameraVertical((int)_verticalSlider.Value);
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}