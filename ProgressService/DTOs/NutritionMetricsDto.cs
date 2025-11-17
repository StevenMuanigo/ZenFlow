namespace ZenFlow.ProgressService.DTOs
{
    public class NutritionMetricsDto
    {
        public int CaloriesConsumed { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal Fat { get; set; }
        public decimal Fiber { get; set; }
        public decimal Water { get; set; }
        public int MealsLogged { get; set; }
        public int SupplementsTaken { get; set; }
    }
}