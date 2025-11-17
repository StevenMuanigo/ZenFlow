namespace ZenFlow.WorkoutService.DTOs
{
    public class CreateExerciseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MuscleGroup { get; set; }
        public string ExerciseType { get; set; }
        public string Equipment { get; set; }
        public List<string> Instructions { get; set; } = new List<string>();
        public string VideoUrl { get; set; }
        public int DefaultSets { get; set; }
        public int DefaultReps { get; set; }
        public decimal DefaultWeight { get; set; }
        public int DefaultDuration { get; set; }
    }
}