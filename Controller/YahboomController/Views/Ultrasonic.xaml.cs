using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YahboomController.ViewModels;

namespace YahboomController.Views
{
    public class Ultrasonic : UserControl
    {
        private readonly Slider _slider;
        public Ultrasonic()
        {
            InitializeComponent();
            _slider = this.FindControl<Slider>("UltrasonicSlider");
            
            _slider.PointerCaptureLost += (sender, args) =>
            {
                RightLeft();
            };
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void RightLeft()
        {
            ((UltrasonicViewModel) this.DataContext)?.Client.SetUltrasonic((int)_slider.Value);
        }
    }
}