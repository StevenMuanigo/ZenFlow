namespace ZenFlow.ProgressService.DTOs
{
    public class ProgressChartDto
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string MetricName { get; set; }
    }
}