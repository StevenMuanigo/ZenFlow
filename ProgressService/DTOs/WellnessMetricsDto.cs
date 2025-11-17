namespace ZenFlow.ProgressService.DTOs
{
    public class WellnessMetricsDto
    {
        public decimal SleepHours { get; set; }
        public int SleepQuality { get; set; }
        public int StressLevel { get; set; }
        public int Mood { get; set; }
        public int EnergyLevel { get; set; }
        public int HydrationLevel { get; set; }
        public int Productivity { get; set; }
    }
}