using System;
using System.Device.Gpio;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using Robot.Configs;

namespace Robot.Devices
{
    public interface ICamera : IDisposable
    {
        byte[] ReadImage();
        void SetHorizontalRadiance(int degree);
        void SetVerticalRadiance(int degree);
        int GetHorizontal();
        int GetVertical();
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

            _videoCapture = new VideoCapture(0);
        }

        public byte[] ReadImage()
        {
            var image = new Mat();
            return _videoCapture.Read(image) ? image.ToImage<Bgr,byte>().ToJpegData() : null;
        }

        public void SetHorizontalRadiance(int degree)
        {
            _horizontal.SetRadiance(degree);
        }

        public void SetVerticalRadiance(int degree)
        {
            _vertical.SetRadiance(degree);
        }

        public int GetHorizontal()
        {
            return _horizontal.GetAngle();
        }

        public int GetVertical()
        {
            return _vertical.GetAngle();
        }

        public void Dispose()
        {
            _videoCapture.Dispose();
            _horizontal.Dispose();
            _vertical.Dispose();
        }
    }
}