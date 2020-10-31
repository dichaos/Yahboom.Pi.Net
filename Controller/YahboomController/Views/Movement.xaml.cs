using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using YahboomController.ViewModels;
using RobotControllerContract;

namespace YahboomController.Views
{
    public class Movement : UserControl
    {
        private readonly Slider _speedSlider;
        
        private ClientViewModel ViewModel => DataContext as ClientViewModel;
        
        public Movement()
        {
            InitializeComponent();
            _speedSlider = this.FindControl<Slider>("SpeedSlider");
            
            _speedSlider.PointerCaptureLost += (sender, args) => ChangeSpeed();
        }

        public async void PointerDownEvent(object sender, Avalonia.Input.PointerPressedEventArgs args)
        {
            MovementRequest.Types.Direction direction = MovementRequest.Types.Direction.Stop;

            
            var button = (Button) ((Image)sender).Parent;

            if (button.Name.Equals("UpButton"))
            {
                direction = MovementRequest.Types.Direction.Forwards;
            }
            else if (button.Name.Equals("DownButton"))
            {
                direction = MovementRequest.Types.Direction.Backwards;
            }
            else if (button.Name.Equals("LeftButton"))
            {
                direction = MovementRequest.Types.Direction.Left;
            }
            else if (button.Name.Equals("RightButton"))
            {
                direction = MovementRequest.Types.Direction.Right;
            }

            await ViewModel.Client.SetMovement(direction);
        }
        
        public async void PointerUpEvent(object sender, Avalonia.Input.PointerReleasedEventArgs args)
        {
            await ViewModel.Client.SetMovement(MovementRequest.Types.Direction.Stop);
        }
        
        private async Task ChangeSpeed()
        {
            await ViewModel.Client.SetMovementSpeed((int)_speedSlider.Value);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}