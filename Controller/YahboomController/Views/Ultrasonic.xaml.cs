using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace YahboomController.Views
{
    public class Ultrasonic : UserControl
    {
        public Ultrasonic()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}