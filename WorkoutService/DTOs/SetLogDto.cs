namespace ZenFlow.WorkoutService.DTOs
{
    public class SetLogDto
    {
        public string Id { get; set; }
        public int SetNumber { get; set; }
        public int Reps { get; set; }
        public decimal Weight { get; set; }
        public int Duration { get; set; }
        public bool Completed { get; set; }
        public string Notes { get; set; }
    }
}