using MongoDB.Driver;
using ZenFlow.CatalogService.Models;

namespace ZenFlow.CatalogService.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("ZenFlowCatalogDb");
            Products = database.GetCollection<Product>("Products");
            SeedData();
        }

        public IMongoCollection<Product> Products { get; }

        private void SeedData()
        {
            // Check if collection is empty, then seed data
            if (Products.CountDocuments(p => true) == 0)
            {
                Products.InsertMany(GetPreconfiguredProducts());
            }
        }

        private IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Name = "Greek Yogurt",
                    Description = "High protein yogurt, perfect for breakfast or snacks",
                    Category = "Dairy",
                    Calories = 120,
                    Protein = 20,
                    Carbohydrates = 6,
                    Fat = 0,
                    ServingSize = "1 cup (245g)",
                    Allergens = new List<string> { "Milk" },
                    DietaryTags = new List<string> { "High Protein" }
                },
                new Product
                {
                    Name = "Almond Butter",
                    Description = "Rich and creamy almond butter, great for spreading or cooking",
                    Category = "Nuts & Seeds",
                    Calories = 196,
                    Protein = 7,
                    Carbohydrates = 6,
                    Fat = 18,
                    ServingSize = "2 tbsp (32g)",
                    Allergens = new List<string> { "Tree Nuts" },
                    DietaryTags = new List<string> { "Vegan", "Gluten-Free" }
                },
                new Product
                {
                    Name = "Quinoa",
                    Description = "Complete protein grain, gluten-free and versatile",
                    Category = "Grains",
                    Calories = 222,
                    Protein = 8,
                    Carbohydrates = 39,
                    Fat = 4,
                    ServingSize = "1 cup cooked (185g)",
                    Allergens = new List<string>(),
                    DietaryTags = new List<string> { "Vegan", "Gluten-Free", "Complete Protein" }
                },
                new Product
                {
                    Name = "Salmon Fillet",
                    Description = "Fresh Atlantic salmon, rich in omega-3 fatty acids",
                    Category = "Fish",
                    Calories = 206,
                    Protein = 22,
                    Carbohydrates = 0,
                    Fat = 13,
                    ServingSize = "1 fillet (154g)",
                    Allergens = new List<string> { "Fish" },
                    DietaryTags = new List<string> { "High Protein", "Omega-3" }
                },
                new Product
                {
                    Name = "Spinach",
                    Description = "Fresh spinach leaves, rich in iron and vitamins",
                    Category = "Vegetables",
                    Calories = 23,
                    Protein = 2.9,
                    Carbohydrates = 3.6,
                    Fat = 0.4,
                    ServingSize = "1 cup (30g)",
                    Allergens = new List<string>(),
                    DietaryTags = new List<string> { "Vegan", "Gluten-Free", "Low Calorie" }
                }
            };
        }
    }
}