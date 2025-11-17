using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.ProgressService.Models
{
    public class NutritionMetrics
    {
        [BsonElement("caloriesConsumed")]
        public int CaloriesConsumed { get; set; }
        
        [BsonElement("protein")]
        public decimal Protein { get; set; } // in grams
        
        [BsonElement("carbohydrates")]
        public decimal Carbohydrates { get; set; } // in grams
        
        [BsonElement("fat")]
        public decimal Fat { get; set; } // in grams
        
        [BsonElement("fiber")]
        public decimal Fiber { get; set; } // in grams
        
        [BsonElement("water")]
        public decimal Water { get; set; } // in liters
        
        [BsonElement("mealsLogged")]
        public int MealsLogged { get; set; }
        
        [BsonElement("supplementsTaken")]
        public int SupplementsTaken { get; set; }
    }
}