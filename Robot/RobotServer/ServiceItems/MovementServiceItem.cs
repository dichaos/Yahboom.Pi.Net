using System;
using Microsoft.Extensions.Logging;
using Robot.Devices;
using RobotControllerContract;
using RobotServer.Services;

namespace RobotServer.ServiceItems
{
    public interface IMovementServiceItem
    {
        Reply Movement(MovementRequest request);
    }

    public class MovementServiceItem : ServiceItemBase, IMovementServiceItem
    {
        private readonly IMovement _movement;
        
        public MovementServiceItem(ILogger<RobotService> logger, IMovement movement):base(logger)
        {
            _movement = movement;
        }
        
        public Reply Movement(MovementRequest request)
        {
            Console.WriteLine(request.MovementDirection+ " "+request.Speed);
            try
            {
                switch (request.MovementDirection)
                {
                    case MovementRequest.Types.Direction.Forwards:
                        _movement.Forward();
                        break;
                    case MovementRequest.Types.Direction.Backwards:
                        _movement.Backward();
                        break;
                    case MovementRequest.Types.Direction.Left:
                        _movement.TurnLeft();
                        break;
                    case MovementRequest.Types.Direction.Right:
                        _movement.TurnRight();
                        break;
                    case MovementRequest.Types.Direction.Stop:
                        _movement.Stop();
                        break;
                    case MovementRequest.Types.Direction.Speed:
                        _movement.SetSpeed(request.Speed);
                        break;
                }

                return new Reply() {Success = true};
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error with movement");
                return new Reply() {Success = false};
            }
        }
    }
}