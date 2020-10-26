using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace YahboomController.Views
{
    public class Tracker : UserControl
    {
        public Tracker()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}