using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.ProgressService.Models
{
    public class WellnessMetrics
    {
        [BsonElement("sleepHours")]
        public decimal SleepHours { get; set; }
        
        [BsonElement("sleepQuality")]
        public int SleepQuality { get; set; } // 1-10 scale
        
        [BsonElement("stressLevel")]
        public int StressLevel { get; set; } // 1-10 scale
        
        [BsonElement("mood")]
        public int Mood { get; set; } // 1-10 scale
        
        [BsonElement("energyLevel")]
        public int EnergyLevel { get; set; } // 1-10 scale
        
        [BsonElement("hydrationLevel")]
        public int HydrationLevel { get; set; } // 1-10 scale
        
        [BsonElement("productivity")]
        public int Productivity { get; set; } // 1-10 scale
    }
}