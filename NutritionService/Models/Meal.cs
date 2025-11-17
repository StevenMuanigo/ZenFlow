using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.NutritionService.Models
{
    public class Meal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; } // Breakfast, Lunch, Dinner, Snack
        
        [BsonElement("time")]
        public string Time { get; set; } // e.g., "08:00"
        
        [BsonElement("foods")]
        public List<FoodItem> Foods { get; set; } = new List<FoodItem>();
        
        [BsonElement("totalCalories")]
        public int TotalCalories { get; set; }
        
        [BsonElement("totalProtein")]
        public decimal TotalProtein { get; set; }
        
        [BsonElement("totalCarbohydrates")]
        public decimal TotalCarbohydrates { get; set; }
        
        [BsonElement("totalFat")]
        public decimal TotalFat { get; set; }
    }
}