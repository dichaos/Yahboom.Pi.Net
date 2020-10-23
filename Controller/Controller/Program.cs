using System;
using System.Threading.Tasks;
using ControllerClient;

namespace Controller
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            Console.WriteLine("Hello World!");
            var c = new Client("http://192.168.1.28:5000 ");
            //var c = new Client("http://localhost:5000");
            await c.SetLED(250, 250, 250);
            
            
        }
    }
}