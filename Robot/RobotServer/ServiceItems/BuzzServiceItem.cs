using System;
using Robot.Devices;
using RobotControllerContract;

namespace RobotServer.ServiceItems
{
    public interface IBuzzServiceItem
    {
        Reply Buzz(BuzzValue request);
    }

    public class BuzzServiceItem : IBuzzServiceItem
    {
        public IBuzzer _buzzer;
        
        public BuzzServiceItem(IBuzzer buzzer)
        {
            _buzzer = buzzer;
        }
        
        public Reply Buzz(BuzzValue request)
        {
            try
            {
                if(request.OnOff)
                    _buzzer.Start();
                else
                {
                    _buzzer.Stop();
                }

                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Reply() {Success = false};
            }
        }
    }
}