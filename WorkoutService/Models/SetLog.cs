using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.WorkoutService.Models
{
    public class SetLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("setNumber")]
        public int SetNumber { get; set; }
        
        [BsonElement("reps")]
        public int Reps { get; set; }
        
        [BsonElement("weight")]
        public decimal Weight { get; set; } // in kg
        
        [BsonElement("duration")]
        public int Duration { get; set; } // in seconds for timed exercises
        
        [BsonElement("completed")]
        public bool Completed { get; set; } = true;
        
        [BsonElement("notes")]
        public string Notes { get; set; }
    }
}