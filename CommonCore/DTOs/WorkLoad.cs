namespace SwimmiePooLogParser.Common.DTOs
{
    public class WorkLoad
    {
        public WorkLoadItem[] WorkLines { get; set; }
    }

    public class WorkLoadItem
    {
        public string LogFile { get; set; }
        public string LogFilePath { get; set; }
        public int LineNumber { get; set; }
        public string WorkLine { get; set; }
    }

    public class DroneAvailability
    {
        public int DroneId { get; set; }
        public string DroneUrl { get; set; }
        public int AvailableSlots { get; set; }
    }

    public class WorkLoadAssignment
    {
        public int DroneId { get; set; }
        public string DroneUrl { get; set; }
        public WorkLoadItem[] Work { get; set; }
    }
}