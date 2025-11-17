using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.NutritionService.Models
{
    public class FoodItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("productId")]
        public string ProductId { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("quantity")]
        public decimal Quantity { get; set; } // in grams or servings
        
        [BsonElement("unit")]
        public string Unit { get; set; } // grams, servings, etc.
        
        [BsonElement("calories")]
        public int Calories { get; set; }
        
        [BsonElement("protein")]
        public decimal Protein { get; set; }
        
        [BsonElement("carbohydrates")]
        public decimal Carbohydrates { get; set; }
        
        [BsonElement("fat")]
        public decimal Fat { get; set; }
    }
}