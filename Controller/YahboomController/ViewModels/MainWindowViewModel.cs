using System.Threading;

namespace YahboomController.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public readonly CancellationTokenSource source;

        public MainWindowViewModel(Client c)
        {
            source = new CancellationTokenSource();
            Ultrasonic = new UltrasonicViewModel(source.Token, c);
            Tracker = new TrackerViewModel(source.Token, c);
            Camera = new CameraViewModel(source.Token, c);
            Client = new ClientViewModel(c);
        }

        public UltrasonicViewModel Ultrasonic { get; }
        public TrackerViewModel Tracker { get; }
        public CameraViewModel Camera { get; }
        public ClientViewModel Client { get; }

        public void Stop()
        {
            source.Cancel();
            WaitHandle.WaitAny(new[] {source.Token.WaitHandle});
        }
    }
}