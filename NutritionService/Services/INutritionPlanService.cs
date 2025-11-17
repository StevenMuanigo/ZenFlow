using ZenFlow.NutritionService.DTOs;
using ZenFlow.NutritionService.Models;

namespace ZenFlow.NutritionService.Services
{
    public interface INutritionPlanService
    {
        Task<NutritionPlanDto> CreateNutritionPlanAsync(CreateNutritionPlanDto planDto);
        Task<NutritionPlanDto> GetNutritionPlanByUserIdAsync(int userId);
        Task<NutritionPlanDto> UpdateNutritionPlanAsync(string id, NutritionPlanDto planDto);
        Task<bool> DeleteNutritionPlanAsync(string id);
        Task<NutritionPlanDto> GeneratePersonalizedPlanAsync(int userId);
    }
}