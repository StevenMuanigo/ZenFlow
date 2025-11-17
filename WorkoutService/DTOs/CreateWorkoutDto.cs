namespace ZenFlow.WorkoutService.DTOs
{
    public class CreateWorkoutDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WorkoutType { get; set; }
        public string DifficultyLevel { get; set; }
        public int EstimatedDuration { get; set; }
        public List<CreateExerciseDto> Exercises { get; set; } = new List<CreateExerciseDto>();
    }
}