using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YahboomController.ViewModels;

namespace YahboomController.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        protected override void OnClosed(EventArgs e)
        {
            ((MainWindowViewModel) DataContext)?.Stop();
            base.OnClosed(e);
        }
    }
}