namespace ZenFlow.WorkoutService.DTOs
{
    public class WorkoutSessionDto
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public string WorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Duration { get; set; }
        public List<ExerciseLogDto> Exercises { get; set; } = new List<ExerciseLogDto>();
        public int? CaloriesBurned { get; set; }
        public string Notes { get; set; }
        public bool Completed { get; set; }
    }
}