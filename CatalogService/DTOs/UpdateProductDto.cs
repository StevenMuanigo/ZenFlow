namespace ZenFlow.CatalogService.DTOs
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fat { get; set; }
        public string ServingSize { get; set; }
        public List<string> Allergens { get; set; } = new List<string>();
        public List<string> DietaryTags { get; set; } = new List<string>();
    }
}