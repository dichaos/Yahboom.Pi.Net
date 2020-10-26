using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace YahboomController.Views
{
    public class Camera : UserControl
    {
        public Camera()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}