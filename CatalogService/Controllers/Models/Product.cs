using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.CatalogService.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("description")]
        public string Description { get; set; }
        
        [BsonElement("category")]
        public string Category { get; set; }
        
        [BsonElement("calories")]
        public int Calories { get; set; }
        
        [BsonElement("protein")]
        public decimal Protein { get; set; } // grams
        
        [BsonElement("carbohydrates")]
        public decimal Carbohydrates { get; set; } // grams
        
        [BsonElement("fat")]
        public decimal Fat { get; set; } // grams
        
        [BsonElement("servingSize")]
        public string ServingSize { get; set; }
        
        [BsonElement("allergens")]
        public List<string> Allergens { get; set; } = new List<string>();
        
        [BsonElement("dietaryTags")]
        public List<string> DietaryTags { get; set; } = new List<string>(); // vegan, vegetarian, gluten-free, etc.
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}