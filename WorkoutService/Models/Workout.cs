using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.WorkoutService.Models
{
    public class Workout
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("userId")]
        public int UserId { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("description")]
        public string Description { get; set; }
        
        [BsonElement("workoutType")]
        public string WorkoutType { get; set; } // Cardio, Strength, Flexibility, etc.
        
        [BsonElement("difficultyLevel")]
        public string DifficultyLevel { get; set; } // Beginner, Intermediate, Advanced
        
        [BsonElement("estimatedDuration")]
        public int EstimatedDuration { get; set; } // in minutes
        
        [BsonElement("exercises")]
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}