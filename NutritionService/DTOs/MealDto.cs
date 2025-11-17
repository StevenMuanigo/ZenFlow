namespace ZenFlow.NutritionService.DTOs
{
    public class MealDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public List<FoodItemDto> Foods { get; set; } = new List<FoodItemDto>();
        public int TotalCalories { get; set; }
        public decimal TotalProtein { get; set; }
        public decimal TotalCarbohydrates { get; set; }
        public decimal TotalFat { get; set; }
    }
}