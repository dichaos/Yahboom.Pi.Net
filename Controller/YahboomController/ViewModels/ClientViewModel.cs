namespace YahboomController.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        public ClientViewModel(Client c)
        {
            Client = c;
        }

        public Client Client { get; }
    }
}