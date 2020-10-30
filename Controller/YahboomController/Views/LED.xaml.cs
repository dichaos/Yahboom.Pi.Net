using System;
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
            
            _RedSlider.WhenValueChanged(x => x.Value).Subscribe(ChangeRed);
            _GreenSlider.WhenValueChanged(x => x.Value).Subscribe(ChangeGreen);
            _BlueSlider.WhenValueChanged(x => x.Value).Subscribe(ChangeBlue);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ChangeRed(double value)
        {
            _RedTextBlock.Background = new SolidColorBrush(new Color(255, (byte) value, 0, 0));
            _ColorTextBlock.Background = new SolidColorBrush(new Color(255, (byte)_RedSlider.Value, (byte)_GreenSlider.Value, (byte)_BlueSlider.Value));
            ((LEDViewModel) this.DataContext)?.Client.SetLED((int) _RedSlider.Value, (int) _GreenSlider.Value, (int) _BlueSlider.Value);
        }
        
        private void ChangeGreen(double value)
        {
            _GreenTextBlock.Background = new SolidColorBrush(new Color(255, 0, (byte) value, 0));
            _ColorTextBlock.Background = new SolidColorBrush(new Color(255, (byte)_RedSlider.Value, (byte)_GreenSlider.Value, (byte)_BlueSlider.Value));
            ((LEDViewModel) this.DataContext)?.Client.SetLED((int) _RedSlider.Value, (int) _GreenSlider.Value, (int) _BlueSlider.Value);
        }

        private void ChangeBlue(double value)
        {
            _BlueTextBlock.Background =new SolidColorBrush(new Color(255,0, 0, (byte)value));
            _ColorTextBlock.Background = new SolidColorBrush(new Color(255, (byte)_RedSlider.Value, (byte)_GreenSlider.Value, (byte)_BlueSlider.Value));
            ((LEDViewModel) this.DataContext)?.Client.SetLED((int) _RedSlider.Value, (int) _GreenSlider.Value, (int) _BlueSlider.Value);
        }
    }
}