using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.ProgressService.Models
{
    public class FitnessMetrics
    {
        [BsonElement("workoutsCompleted")]
        public int WorkoutsCompleted { get; set; }
        
        [BsonElement("totalWorkoutTime")]
        public int TotalWorkoutTime { get; set; } // in minutes
        
        [BsonElement("caloriesBurned")]
        public int CaloriesBurned { get; set; }
        
        [BsonElement("steps")]
        public int Steps { get; set; }
        
        [BsonElement("distance")]
        public decimal Distance { get; set; } // in km
        
        [BsonElement("activeMinutes")]
        public int ActiveMinutes { get; set; }
        
        [BsonElement("restingHeartRate")]
        public int? RestingHeartRate { get; set; } // in bpm
        
        [BsonElement("maxHeartRate")]
        public int? MaxHeartRate { get; set; } // in bpm
        
        [BsonElement("vo2Max")]
        public decimal? Vo2Max { get; set; }
    }
}