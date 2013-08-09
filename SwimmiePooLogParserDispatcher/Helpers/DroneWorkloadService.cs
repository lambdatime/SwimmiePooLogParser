using SwimmiePooLogParser.Common.DTOs;

namespace SwimmiePooLogParserDispatcher.Helpers
{
    public class DroneWorkloadService : JsonServiceClient
    {
        public DroneWorkloadService(string droneServiceUrl) : base(droneServiceUrl)
        {
        }

        public int GetAvailableQueueCount()
        {
            return Get<int>("WorkLoad/GetAvailableQueueCount");
        }

        public int GetProcessingCount()
        {
            return Get<int>("WorkLoad/GetProcessingCount");
        }

        public int GetQueuedCount()
        {
            return Get<int>("WorkLoad/GetQueuedCount");
        }

        public bool SetMaxProcessingCount(int maxProcessingCount)
        {
            return Post<bool>("WorkLoad/SetMaxProcessingCount", maxProcessingCount);
        }

        public bool SetMaxQueueCount(int maxQueueCount)
        {
            return Post<bool>("WorkLoad/SetMaxQueueCount", maxQueueCount);
        }

        public int BeginProcessingLoad(WorkLoad workLoad)
        {
            return Post<int>("WorkLoad/BeginProcessingLoad", workLoad);
        }
    }
}