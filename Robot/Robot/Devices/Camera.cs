using System;
using System.Device.Gpio;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;
using Robot.Configs;

namespace Robot.Devices
{
    public interface ICamera : IDisposable
    {
        byte[] ReadImage();
        void SetHorizontal(int degree);
        void SetVertical(int degree);
    }

    public class Camera : IDisposable, ICamera
    {
        private readonly Servo _horizontal;
        private readonly Servo _vertical;
        private readonly VideoCapture _videoCapture;
        
        public Camera(CameraSettings cameraSettings, GpioController gpioController)
        {
            _horizontal = new Servo(cameraSettings.CameraHorizontal, gpioController);
            _vertical = new Servo(cameraSettings.CameraVertical, gpioController);

            _videoCapture = new VideoCapture();
            
            ListCameras();
        }

        public byte[] ReadImage()
        {
            var image = new Mat();
            return _videoCapture.Read(image) ? image.ToImage<Bgr, byte>().ToJpegData() : null;
        }

        public void SetHorizontal(int degree)
        {
            _horizontal.SetDutyCycle(degree);
        }

        public void SetVertical(int degree)
        {
            _vertical.SetDutyCycle(degree);
        }


        public void Dispose()
        {
            _videoCapture.Dispose();
        }

        private void ListCameras()
        {
            try
            {
                var cmd = "vcgencmd get_camera";
                var escapedArgs = cmd.Replace("\"", "\\\"");

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
                process.Start();
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch
            {
                // ignored
            }
        }
    }
}