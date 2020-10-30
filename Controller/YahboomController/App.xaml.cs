using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using YahboomController.ViewModels;
using YahboomController.Views;

namespace YahboomController
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(new Client("http://192.168.1.28:5000")),
                    //DataContext = new MainWindowViewModel(new Client("http://127.0.0.1:5001")),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}