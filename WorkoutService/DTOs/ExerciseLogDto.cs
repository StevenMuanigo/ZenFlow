namespace ZenFlow.WorkoutService.DTOs
{
    public class ExerciseLogDto
    {
        public string Id { get; set; }
        public string ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public List<SetLogDto> Sets { get; set; } = new List<SetLogDto>();
        public string Notes { get; set; }
    }
}