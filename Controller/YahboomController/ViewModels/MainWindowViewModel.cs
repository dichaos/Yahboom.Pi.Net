using System.Threading;

namespace YahboomController.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public readonly CancellationTokenSource source;
        public UltrasonicViewModel Ultrasonic { get; private set; }
        public TrackerViewModel Tracker { get; private set; }
        public CameraViewModel Camera { get; private set; }
        public LEDViewModel Led { get; private set; }
        
        public MainWindowViewModel(Client c)
        {
            source = new CancellationTokenSource();
            Ultrasonic = new UltrasonicViewModel(source.Token, c);
            Tracker = new TrackerViewModel(source.Token, c);
            Camera = new CameraViewModel(source.Token, c);
            Led = new LEDViewModel(c);
        }

        public void Stop()
        {
            source.Cancel();
            WaitHandle.WaitAny(new[] { source.Token.WaitHandle });
        }
    }
}