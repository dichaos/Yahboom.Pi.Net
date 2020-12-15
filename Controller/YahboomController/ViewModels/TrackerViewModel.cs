using System.Threading;
using ReactiveUI;
using RobotControllerContract;

namespace YahboomController.ViewModels
{
    public class TrackerViewModel : ViewModelBase
    {
        private string _leftPin1;
        private string _leftPin2;
        private string _rightPin1;
        private string _rightPin2;

        public TrackerViewModel(CancellationToken token, Client c)
        {
            var task = c.GetTrackerValues(token, Process);
            task.Start();
        }

        public string RightPin1
        {
            get => _rightPin1;
            set => this.RaiseAndSetIfChanged(ref _rightPin1, value);
        }

        public string RightPin2
        {
            get => _rightPin2;
            set => this.RaiseAndSetIfChanged(ref _rightPin2, value);
        }

        public string LeftPin1
        {
            get => _leftPin1;
            set => this.RaiseAndSetIfChanged(ref _leftPin1, value);
        }

        public string LeftPin2
        {
            get => _leftPin2;
            set => this.RaiseAndSetIfChanged(ref _leftPin2, value);
        }

        private void Process(TrackerData value)
        {
            LeftPin1 = value.LeftPin1 == false ? "White" : "Red";
            LeftPin2 = value.LeftPin2 == false ? "White" : "Red";
            RightPin1 = value.RightPin1 == false ? "White" : "Red";
            RightPin2 = value.RightPin2 == false ? "White" : "Red";
        }
    }
}