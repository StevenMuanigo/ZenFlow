using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.NutritionService.Models
{
    public class NutritionPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("userId")]
        public int UserId { get; set; }
        
        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }
        
        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }
        
        [BsonElement("dailyCalorieTarget")]
        public int DailyCalorieTarget { get; set; }
        
        [BsonElement("dailyProteinTarget")]
        public decimal DailyProteinTarget { get; set; }
        
        [BsonElement("dailyCarbohydrateTarget")]
        public decimal DailyCarbohydrateTarget { get; set; }
        
        [BsonElement("dailyFatTarget")]
        public decimal DailyFatTarget { get; set; }
        
        [BsonElement("meals")]
        public List<Meal> Meals { get; set; } = new List<Meal>();
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}