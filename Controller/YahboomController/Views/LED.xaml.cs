using System;
using ABI.Windows.Media.Protection.PlayReady;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DynamicData.Binding;
using ReactiveUI;
using YahboomController.ViewModels;

namespace YahboomController.Views
{
    public class LED : UserControl
    {
        private readonly Slider _RedSlider;
        private readonly Slider _BlueSlider;
        private readonly Slider _GreenSlider;

        private readonly TextBlock _RedTextBlock;
        private readonly TextBlock _BlueTextBlock;
        private readonly TextBlock _GreenTextBlock;
        private readonly TextBlock _ColorTextBlock;
        
        public LED()
        {
            InitializeComponent();
            
            _RedSlider = this.FindControl<Slider>("RedSlider");
            _GreenSlider = this.FindControl<Slider>("GreenSlider");
            _BlueSlider = this.FindControl<Slider>("BlueSlider");
            
            _RedTextBlock = this.FindControl<TextBlock>("RedText");
            _BlueTextBlock = this.FindControl<TextBlock>("BlueText");
            _GreenTextBlock = this.FindControl<TextBlock>("GreenText");

            _ColorTextBlock = this.FindControl<TextBlock>("ColorText");

            _RedSlider.PointerCaptureLost += (sender, args) =>
            {
                ChangeRed();
            };
            
            _GreenSlider.PointerCaptureLost += (sender, args) =>
            {
                ChangeGreen();
            };

            _BlueSlider.PointerCaptureLost += (sender, args) =>
            {
                ChangeBlue();
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ChangeRed()
        {
            _RedTextBlock.Background = new SolidColorBrush(new Color(255, (byte) _RedSlider.Value, 0, 0));
            _ColorTextBlock.Background = new SolidColorBrush(new Color(255, (byte)_RedSlider.Value, (byte)_GreenSlider.Value, (byte)_BlueSlider.Value));
            ((LEDViewModel) this.DataContext)?.Client.SetLED((int) _RedSlider.Value, (int) _GreenSlider.Value, (int) _BlueSlider.Value);
        }
        
        private void ChangeGreen()
        {
            _GreenTextBlock.Background = new SolidColorBrush(new Color(255, 0, (byte)_GreenSlider.Value, 0));
            _ColorTextBlock.Background = new SolidColorBrush(new Color(255, (byte)_RedSlider.Value, (byte)_GreenSlider.Value, (byte)_BlueSlider.Value));
            ((LEDViewModel) this.DataContext)?.Client.SetLED((int) _RedSlider.Value, (int) _GreenSlider.Value, (int) _BlueSlider.Value);
        }

        private void ChangeBlue()
        {
            _BlueTextBlock.Background =new SolidColorBrush(new Color(255,0, 0, (byte)_BlueSlider.Value));
            _ColorTextBlock.Background = new SolidColorBrush(new Color(255, (byte)_RedSlider.Value, (byte)_GreenSlider.Value, (byte)_BlueSlider.Value));
            ((LEDViewModel) this.DataContext)?.Client.SetLED((int) _RedSlider.Value, (int) _GreenSlider.Value, (int) _BlueSlider.Value);
        }
    }
}