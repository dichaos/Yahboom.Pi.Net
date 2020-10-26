using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace YahboomController.Views
{
    public class Recording : UserControl
    {
        public Recording()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}