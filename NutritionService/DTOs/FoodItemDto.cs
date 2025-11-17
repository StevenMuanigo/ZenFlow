namespace ZenFlow.NutritionService.DTOs
{
    public class FoodItemDto
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public int Calories { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fat { get; set; }
    }
}