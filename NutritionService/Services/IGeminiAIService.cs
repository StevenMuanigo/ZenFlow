using ZenFlow.NutritionService.Models;

namespace ZenFlow.NutritionService.Services
{
    public interface IGeminiAIService
    {
        Task<string> GenerateMealPlanAsync(UserProfile userProfile, NutritionPlan nutritionPlan);
        Task<string> GenerateRecipeAsync(List<FoodItem> ingredients);
        Task<string> GetNutritionAdviceAsync(UserProfile userProfile, string concern);
    }
}