using System;
using System.Device.Gpio;
using Iot.Device.DCMotor;
using Iot.Device.ExplorerHat;
using Robot.Configs;

namespace Robot.Devices
{
    public interface IMovement : IDisposable
    {
        void Forward();
        void Backward();
        void TurnLeft();
        void TurnRight();
        void SetSpeed(double speed);
        void Stop();
        int GetSpeed();
    }

    public class Movement : IMovement
    {
        private readonly DCMotor _motor1;
        private readonly DCMotor _motor2;

        public Movement(MovementSettings movementSettings, GpioController gpioController)
        {
            _motor1 = DCMotor.Create(movementSettings.Left.ENA, movementSettings.Left.IN1,movementSettings.Left.IN2, gpioController);
            _motor2 = DCMotor.Create(movementSettings.Right.ENA, movementSettings.Right.IN1,movementSettings.Right.IN2, gpioController);
        }

        public void Forward()
        {
            _motor1.Forwards();
            _motor2.Forwards();
        }

        public void Backward()
        {
            _motor1.Backwards();
            _motor2.Backwards();
        }

        public void TurnLeft()
        {
            _motor1.Forwards();
            _motor2.Backwards();
        }
        
        public void TurnRight()
        {
            _motor1.Backwards();
            _motor2.Forwards();
        }

        public void SetSpeed(double speed)
        {
            _motor1.Speed = speed;
            _motor2.Speed = speed;
        }

        public void Stop()
        {
            try
            {
                _motor1.Stop();
                _motor2.Stop();
            }
            catch
            {
            }
        }

        public int GetSpeed()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Stop();
            
            _motor1.Dispose();
            _motor2.Dispose();
        }
    }
}