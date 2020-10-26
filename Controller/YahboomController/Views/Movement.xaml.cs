using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace YahboomController.Views
{
    public class Movement : UserControl
    {
        public Movement()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}