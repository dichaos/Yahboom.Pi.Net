using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YahboomController.ViewModels;

namespace YahboomController.Views
{
    public class Ultrasonic : UserControl
    {
        private readonly Slider _slider;
        private readonly Button _button;

        public Ultrasonic()
        {
            InitializeComponent();
            _slider = this.FindControl<Slider>("UltrasonicSlider");
            _button = this.FindControl<Button>("CenterButton");
            _slider.PointerCaptureLost += async (sender, args) => { await RightLeft(); };
            
            _button.Click += async (sender, args) => { await Center(); };
        }

        private async Task Center()
        {
            _slider.Value = 1500;
            await RightLeft();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task RightLeft()
        {
            // ReSharper disable once PossibleNullReferenceException
            await (DataContext as UltrasonicViewModel)?.Client?.SetUltrasonic(_slider.Value);
        }
    }
}