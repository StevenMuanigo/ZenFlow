using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.ProgressService.Models
{
    public class ProgressRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("userId")]
        public int UserId { get; set; }
        
        [BsonElement("date")]
        public DateTime Date { get; set; }
        
        [BsonElement("weight")]
        public decimal? Weight { get; set; } // in kg
        
        [BsonElement("bodyMeasurements")]
        public BodyMeasurements BodyMeasurements { get; set; }
        
        [BsonElement("fitnessMetrics")]
        public FitnessMetrics FitnessMetrics { get; set; }
        
        [BsonElement("nutritionMetrics")]
        public NutritionMetrics NutritionMetrics { get; set; }
        
        [BsonElement("wellnessMetrics")]
        public WellnessMetrics WellnessMetrics { get; set; }
        
        [BsonElement("notes")]
        public string Notes { get; set; }
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}