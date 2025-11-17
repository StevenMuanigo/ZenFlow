using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.WorkoutService.Models
{
    public class ExerciseLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("exerciseId")]
        public string ExerciseId { get; set; }
        
        [BsonElement("exerciseName")]
        public string ExerciseName { get; set; }
        
        [BsonElement("sets")]
        public List<SetLog> Sets { get; set; } = new List<SetLog>();
        
        [BsonElement("notes")]
        public string Notes { get; set; }
    }
}