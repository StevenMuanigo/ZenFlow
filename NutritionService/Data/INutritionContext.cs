using MongoDB.Driver;
using ZenFlow.NutritionService.Models;

namespace ZenFlow.NutritionService.Data
{
    public interface INutritionContext
    {
        IMongoCollection<NutritionPlan> NutritionPlans { get; }
    }
}