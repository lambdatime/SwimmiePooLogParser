using SwimmiePooLogParserDispatcher.Core;

namespace SwimmiePooLogParserDispatcher.Models
{
    public class DroneDisplay
    {
        public DroneDisplay()
        {
        }

        public DroneDisplay(Drone drone)
        {
            Id = drone.DroneId;
            Title = drone.Title;
            BaseUrl = drone.BaseUrl;
            SortOrder = drone.SortOrder;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string BaseUrl { get; set; }

        public int SortOrder { get; set; }
    }
}