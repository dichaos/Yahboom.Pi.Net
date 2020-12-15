using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace YahboomController.ViewModels
{
    public class UltrasonicViewModel : ClientViewModel
    {
        private string _ultrasonicValue;

        public UltrasonicViewModel(CancellationToken token, Client c) : base(c)
        {
            var task = c.GetUltrasonic(token, Process);
            task.Start();
        }

        public string UltrasonicValue
        {
            get => _ultrasonicValue;
            set => this.RaiseAndSetIfChanged(ref _ultrasonicValue, value);
        }

        private void Process(double value)
        {
            UltrasonicValue = value + "cm";
        }

        public async Task SetAngle(int angle)
        {
            await Client.SetUltrasonic(angle);
        }
    }
}