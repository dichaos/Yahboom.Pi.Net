using System;

namespace YahboomController.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        public Client Client { get; private set; }

        public ClientViewModel(Client c)
        {
            Client = c;
        }
    }
}