using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.WorkoutService.Models
{
    public class WorkoutSession
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("userId")]
        public int UserId { get; set; }
        
        [BsonElement("workoutId")]
        public string WorkoutId { get; set; }
        
        [BsonElement("workoutName")]
        public string WorkoutName { get; set; }
        
        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }
        
        [BsonElement("endDate")]
        public DateTime? EndDate { get; set; }
        
        [BsonElement("duration")]
        public int? Duration { get; set; } // in minutes
        
        [BsonElement("exercises")]
        public List<ExerciseLog> Exercises { get; set; } = new List<ExerciseLog>();
        
        [BsonElement("caloriesBurned")]
        public int? CaloriesBurned { get; set; }
        
        [BsonElement("notes")]
        public string Notes { get; set; }
        
        [BsonElement("completed")]
        public bool Completed { get; set; } = false;
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}