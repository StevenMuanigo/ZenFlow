namespace ZenFlow.ProgressService.DTOs
{
    public class FitnessMetricsDto
    {
        public int WorkoutsCompleted { get; set; }
        public int TotalWorkoutTime { get; set; }
        public int CaloriesBurned { get; set; }
        public int Steps { get; set; }
        public decimal Distance { get; set; }
        public int ActiveMinutes { get; set; }
        public int? RestingHeartRate { get; set; }
        public int? MaxHeartRate { get; set; }
        public decimal? Vo2Max { get; set; }
    }
}