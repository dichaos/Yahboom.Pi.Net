using System.Threading.Tasks;
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
            
            _slider.PointerCaptureLost += async (sender, args) =>
            {
                await RightLeft();
            };
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task RightLeft()
        {
            await (DataContext as UltrasonicViewModel)?.Client?.SetUltrasonic(_slider.Value);
        }
    }
}