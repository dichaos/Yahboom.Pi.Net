using System.Threading;

namespace YahboomController.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public readonly CancellationTokenSource source;
        public UltrasonicViewModel Ultrasonic { get; set; }
        public TrackerViewModel Tracker { get; set; }
        
        public CameraViewModel Camera { get; set; }
        public MainWindowViewModel(Client c)
        {
            source = new CancellationTokenSource();
            Ultrasonic = new UltrasonicViewModel(source.Token, c);
            Tracker = new TrackerViewModel(source.Token, c);
            Camera = new CameraViewModel(source.Token, c);
        }


        public void Stop()
        {
            source.Cancel();
            WaitHandle.WaitAny(new[] { source.Token.WaitHandle });
        }
    }
}