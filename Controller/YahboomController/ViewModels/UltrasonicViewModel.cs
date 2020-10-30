using System;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace YahboomController.ViewModels
{
    public class UltrasonicViewModel : ViewModelBase
    {
        private string _ultrasonicValue;
        private readonly Client _client;
        public UltrasonicViewModel(CancellationToken token,  Client c)
        {
            _client = c;
            var task = c.GetUltrasonic(token, Process);
            task.Start();
        }

        private void Process(double value)
        {
            UltrasonicValue = value+"cm";
        }

        public string UltrasonicValue
        {
            get => _ultrasonicValue;
            set => this.RaiseAndSetIfChanged(ref _ultrasonicValue, value);
        }

        public async Task SetAngle(int angle)
        {
            await _client.SetUltrasonic(angle);
        }
    }
}
