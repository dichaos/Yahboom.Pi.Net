using System;
using System.Threading;
using System.Threading.Tasks;
using ControllerClient;
using Robot;

namespace Controller
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var c = new Client("http://192.168.1.28:5000");
            
            //await c.SetLED(250, 250, 250);
            //await c.Buzz(true);
            //Task.Delay(1000).Wait();
            //await c.Buzz(false);
            //await c.SetUltrasonic(180);
            //await c.SetCameraHorizontal(180);
            //await c.SetCameraVertical(180);
            
            //CancellationTokenSource source = new CancellationTokenSource();
            
            //var ultra = new Action<double>(Console.WriteLine);
            //var ultraSonicTask = c.GetUltrasonic(source.Token, ultra);
            //ultraSonicTask.Start();
            
            // var tracker = new Action<TrackerData>((v) =>
            // {
            //     Console.WriteLine(v.LeftPin1+","+v.LeftPin2+","+v.RightPin1+","+v.RightPin2);
            // });
            // var trackerTask = c.GetTrackerValues(source.Token, tracker);
            // trackerTask.Start();
            
            // var video = new Action<byte[]>((v) =>
            // {
            //     Console.WriteLine("Received frame");
            // });
            // var videoTask = c.GetVideo(source.Token, video);
            // videoTask.Start();

            //var audio = new Action<short[]>((v) =>
            //{
            //    Console.WriteLine("Audio frame received");
            //});
            //var audioTask = c.GetAudio(source.Token, audio);
            //audioTask.Start();
            
            Console.ReadLine();
            //source.Cancel();
            
            Console.WriteLine("Bye!");
        }
    }
}