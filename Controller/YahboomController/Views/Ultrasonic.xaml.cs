using System;
using Windows.Media.Devices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
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
            _slider.WhenValueChanged(x => x.Value).Subscribe(RightLeft);
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void RightLeft(double value)
        {
            ((UltrasonicViewModel) this.DataContext)?.Client.SetUltrasonic((int)value);
        }
    }
}