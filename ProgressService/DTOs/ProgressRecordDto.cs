namespace ZenFlow.ProgressService.DTOs
{
    public class ProgressRecordDto
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal? Weight { get; set; }
        public BodyMeasurementsDto BodyMeasurements { get; set; }
        public FitnessMetricsDto FitnessMetrics { get; set; }
        public NutritionMetricsDto NutritionMetrics { get; set; }
        public WellnessMetricsDto WellnessMetrics { get; set; }
        public string Notes { get; set; }
    }
}