namespace ZenFlow.ProgressService.DTOs
{
    public class ProgressSummaryDto
    {
        public int TotalRecords { get; set; }
        public decimal? CurrentWeight { get; set; }
        public decimal? WeightChange { get; set; }
        public decimal? CurrentBodyFatPercentage { get; set; }
        public int TotalWorkouts { get; set; }
        public int TotalWorkoutTime { get; set; }
        public int TotalCaloriesBurned { get; set; }
        public int TotalSteps { get; set; }
        public decimal AverageSleepHours { get; set; }
        public int AverageSleepQuality { get; set; }
        public int AverageStressLevel { get; set; }
        public int AverageMood { get; set; }
        public DateTime LastRecordDate { get; set; }
    }
}