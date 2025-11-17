using MongoDB.Driver;
using MongoDB.Bson;
using ZenFlow.NutritionService.Data;
using ZenFlow.NutritionService.DTOs;
using ZenFlow.NutritionService.Models;

namespace ZenFlow.NutritionService.Services
{
    public class NutritionPlanService : INutritionPlanService
    {
        private readonly INutritionContext _context;
        private readonly IGeminiAIService _geminiAIService;

        public NutritionPlanService(INutritionContext context, IGeminiAIService geminiAIService)
        {
            _context = context;
            _geminiAIService = geminiAIService;
        }

        public async Task<NutritionPlanDto> CreateNutritionPlanAsync(CreateNutritionPlanDto planDto)
        {
            var nutritionPlan = new NutritionPlan
            {
                UserId = planDto.UserId,
                StartDate = planDto.StartDate,
                EndDate = planDto.EndDate,
                DailyCalorieTarget = planDto.DailyCalorieTarget,
                DailyProteinTarget = planDto.DailyProteinTarget,
                DailyCarbohydrateTarget = planDto.DailyCarbohydrateTarget,
                DailyFatTarget = planDto.DailyFatTarget
            };

            await _context.NutritionPlans.InsertOneAsync(nutritionPlan);
            return MapToDto(nutritionPlan);
        }

        public async Task<NutritionPlanDto> GetNutritionPlanByUserIdAsync(int userId)
        {
            var nutritionPlan = await _context.NutritionPlans
                .Find(p => p.UserId == userId)
                .SortByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            return nutritionPlan == null ? null : MapToDto(nutritionPlan);
        }

        public async Task<NutritionPlanDto> UpdateNutritionPlanAsync(string id, NutritionPlanDto planDto)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid nutrition plan ID format");
            }

            var update = Builders<NutritionPlan>.Update
                .Set(p => p.StartDate, planDto.StartDate)
                .Set(p => p.EndDate, planDto.EndDate)
                .Set(p => p.DailyCalorieTarget, planDto.DailyCalorieTarget)
                .Set(p => p.DailyProteinTarget, planDto.DailyProteinTarget)
                .Set(p => p.DailyCarbohydrateTarget, planDto.DailyCarbohydrateTarget)
                .Set(p => p.DailyFatTarget, planDto.DailyFatTarget)
                .Set(p => p.Meals, planDto.Meals.Select(m => MapToMealModel(m)).ToList())
                .Set(p => p.UpdatedAt, DateTime.UtcNow);

            var result = await _context.NutritionPlans.UpdateOneAsync(p => p.Id == id, update);
            if (result.ModifiedCount == 0)
            {
                return null;
            }

            var updatedPlan = await _context.NutritionPlans.Find(p => p.Id == id).FirstOrDefaultAsync();
            return MapToDto(updatedPlan);
        }

        public async Task<bool> DeleteNutritionPlanAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid nutrition plan ID format");
            }

            var result = await _context.NutritionPlans.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<NutritionPlanDto> GeneratePersonalizedPlanAsync(int userId)
        {
            // This would typically involve calling UserService to get user profile
            // and then using that information to generate a personalized plan
            // For now, we'll create a basic plan
            var planDto = new CreateNutritionPlanDto
            {
                UserId = userId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                DailyCalorieTarget = 2000,
                DailyProteinTarget = 150,
                DailyCarbohydrateTarget = 250,
                DailyFatTarget = 70
            };

            return await CreateNutritionPlanAsync(planDto);
        }

        private NutritionPlanDto MapToDto(NutritionPlan plan)
        {
            return new NutritionPlanDto
            {
                Id = plan.Id,
                UserId = plan.UserId,
                StartDate = plan.StartDate,
                EndDate = plan.EndDate,
                DailyCalorieTarget = plan.DailyCalorieTarget,
                DailyProteinTarget = plan.DailyProteinTarget,
                DailyCarbohydrateTarget = plan.DailyCarbohydrateTarget,
                DailyFatTarget = plan.DailyFatTarget,
                Meals = plan.Meals.Select(m => MapToMealDto(m)).ToList()
            };
        }

        private MealDto MapToMealDto(Meal meal)
        {
            return new MealDto
            {
                Id = meal.Id,
                Name = meal.Name,
                Time = meal.Time,
                Foods = meal.Foods.Select(f => MapToFoodItemDto(f)).ToList(),
                TotalCalories = meal.TotalCalories,
                TotalProtein = meal.TotalProtein,
                TotalCarbohydrates = meal.TotalCarbohydrates,
                TotalFat = meal.TotalFat
            };
        }

        private FoodItemDto MapToFoodItemDto(FoodItem foodItem)
        {
            return new FoodItemDto
            {
                Id = foodItem.Id,
                ProductId = foodItem.ProductId,
                Name = foodItem.Name,
                Quantity = foodItem.Quantity,
                Unit = foodItem.Unit,
                Calories = foodItem.Calories,
                Protein = foodItem.Protein,
                Carbohydrates = foodItem.Carbohydrates,
                Fat = foodItem.Fat
            };
        }

        private Meal MapToMealModel(MealDto mealDto)
        {
            return new Meal
            {
                Id = mealDto.Id,
                Name = mealDto.Name,
                Time = mealDto.Time,
                Foods = mealDto.Foods.Select(f => MapToFoodItemModel(f)).ToList(),
                TotalCalories = mealDto.TotalCalories,
                TotalProtein = mealDto.TotalProtein,
                TotalCarbohydrates = mealDto.TotalCarbohydrates,
                TotalFat = mealDto.TotalFat
            };
        }

        private FoodItem MapToFoodItemModel(FoodItemDto foodItemDto)
        {
            return new FoodItem
            {
                Id = foodItemDto.Id,
                ProductId = foodItemDto.ProductId,
                Name = foodItemDto.Name,
                Quantity = foodItemDto.Quantity,
                Unit = foodItemDto.Unit,
                Calories = foodItemDto.Calories,
                Protein = foodItemDto.Protein,
                Carbohydrates = foodItemDto.Carbohydrates,
                Fat = foodItemDto.Fat
            };
        }
    }
}