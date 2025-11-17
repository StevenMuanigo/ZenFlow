using MongoDB.Driver;
using ZenFlow.NutritionService.Models;

namespace ZenFlow.NutritionService.Data
{
    public class NutritionContext : INutritionContext
    {
        public NutritionContext(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("ZenFlowNutritionDb");
            NutritionPlans = database.GetCollection<NutritionPlan>("NutritionPlans");
        }

        public IMongoCollection<NutritionPlan> NutritionPlans { get; }
    }
}