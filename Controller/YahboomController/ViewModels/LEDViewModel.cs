using System;

namespace YahboomController.ViewModels
{
    public class LEDViewModel : ViewModelBase
    {
        public Client Client { get; private set; }

        public LEDViewModel(Client c)
        {
            Client = c;
        }
    }
}