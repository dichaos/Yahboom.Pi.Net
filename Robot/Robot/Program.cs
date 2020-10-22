using System;
using System.Runtime.InteropServices;

namespace Robot
{
    
    class Program
    {
        private static Robot robot = null;
        
        static void Main(string[] args)
        {
            Console.CancelKeyPress += myHandler;
            
            try
            {
                robot = new Robot();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                robot?.Dispose();
            }
        }

        private static void myHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Disposing robot");
            robot?.Dispose();
        }
    }
}