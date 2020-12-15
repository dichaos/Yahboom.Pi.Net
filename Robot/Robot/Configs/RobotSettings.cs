using System.IO;
using System.Text.Json;

namespace Robot.Configs
{
    public class RobotSettings
    {
        public int Buzzer { get; set; }
        public LEDSettings LEDSettings { get; set; }
        public TrackerSettings TrackerSettings { get; set; }
        public UltrasonicSettings UltrasonicSettings { get; set; }
        public CameraSettings CameraSettings { get; set; }
        public MovementSettings MovementSettings { get; set; }
        public AudioSettings AudioSettings { get; set; }

        public static RobotSettings GetConfig(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<RobotSettings>(json);
        }
    }
}