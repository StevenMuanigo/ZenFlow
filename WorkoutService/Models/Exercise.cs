using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.WorkoutService.Models
{
    public class Exercise
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("description")]
        public string Description { get; set; }
        
        [BsonElement("muscleGroup")]
        public string MuscleGroup { get; set; } // Chest, Back, Legs, Arms, etc.
        
        [BsonElement("exerciseType")]
        public string ExerciseType { get; set; } // Cardio, Strength, Flexibility, etc.
        
        [BsonElement("equipment")]
        public string Equipment { get; set; } // Dumbbell, Barbell, Machine, Bodyweight, etc.
        
        [BsonElement("instructions")]
        public List<string> Instructions { get; set; } = new List<string>();
        
        [BsonElement("videoUrl")]
        public string VideoUrl { get; set; }
        
        [BsonElement("defaultSets")]
        public int DefaultSets { get; set; }
        
        [BsonElement("defaultReps")]
        public int DefaultReps { get; set; }
        
        [BsonElement("defaultWeight")]
        public decimal DefaultWeight { get; set; } // in kg
        
        [BsonElement("defaultDuration")]
        public int DefaultDuration { get; set; } // in seconds for timed exercises
    }
}