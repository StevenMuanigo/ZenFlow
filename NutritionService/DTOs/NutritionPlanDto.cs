namespace ZenFlow.NutritionService.DTOs
{
    public class NutritionPlanDto
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DailyCalorieTarget { get; set; }
        public decimal DailyProteinTarget { get; set; }
        public decimal DailyCarbohydrateTarget { get; set; }
        public decimal DailyFatTarget { get; set; }
        public List<MealDto> Meals { get; set; } = new List<MealDto>();
    }
}