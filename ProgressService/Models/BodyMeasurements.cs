using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.ProgressService.Models
{
    public class BodyMeasurements
    {
        [BsonElement("height")]
        public decimal? Height { get; set; } // in cm
        
        [BsonElement("chest")]
        public decimal? Chest { get; set; } // in cm
        
        [BsonElement("waist")]
        public decimal? Waist { get; set; } // in cm
        
        [BsonElement("hips")]
        public decimal? Hips { get; set; } // in cm
        
        [BsonElement("arm")]
        public decimal? Arm { get; set; } // in cm
        
        [BsonElement("thigh")]
        public decimal? Thigh { get; set; } // in cm
        
        [BsonElement("bodyFatPercentage")]
        public decimal? BodyFatPercentage { get; set; } // in percentage
    }
}