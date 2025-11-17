using Google.Cloud.AIPlatform.V1;
using System.Text;
using ZenFlow.NutritionService.Models;

namespace ZenFlow.NutritionService.Services
{
    public class GeminiAIService : IGeminiAIService
    {
        private readonly string _apiKey;
        private readonly PredictionServiceClient _predictionServiceClient;
        private readonly string _projectId;
        private readonly string _location;
        private readonly string _modelId;

        public GeminiAIService(IConfiguration configuration)
        {
            _apiKey = configuration["Gemini:ApiKey"];
            _projectId = configuration["Gemini:ProjectId"];
            _location = configuration["Gemini:Location"];
            _modelId = configuration["Gemini:ModelId"];
            
            // Initialize the PredictionServiceClient
            _predictionServiceClient = PredictionServiceClient.Create();
        }

        public async Task<string> GenerateMealPlanAsync(UserProfile userProfile, NutritionPlan nutritionPlan)
        {
            var prompt = BuildMealPlanPrompt(userProfile, nutritionPlan);
            return await CallGeminiAPIAsync(prompt);
        }

        public async Task<string> GenerateRecipeAsync(List<FoodItem> ingredients)
        {
            var prompt = BuildRecipePrompt(ingredients);
            return await CallGeminiAPIAsync(prompt);
        }

        public async Task<string> GetNutritionAdviceAsync(UserProfile userProfile, string concern)
        {
            var prompt = BuildNutritionAdvicePrompt(userProfile, concern);
            return await CallGeminiAPIAsync(prompt);
        }

        private string BuildMealPlanPrompt(UserProfile userProfile, NutritionPlan nutritionPlan)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Generate a personalized meal plan based on the following user profile and nutrition targets:");
            sb.AppendLine($"User Profile:");
            sb.AppendLine($"- Age: {userProfile.Age}");
            sb.AppendLine($"- Gender: {userProfile.Gender}");
            sb.AppendLine($"- Height: {userProfile.Height} cm");
            sb.AppendLine($"- Weight: {userProfile.Weight} kg");
            sb.AppendLine($"- Activity Level: {userProfile.ActivityLevel?.Name}");
            sb.AppendLine($"- Dietary Preferences: {userProfile.DietaryPreferences}");
            sb.AppendLine($"- Allergies: {userProfile.Allergies}");
            
            sb.AppendLine($"\nNutrition Targets:");
            sb.AppendLine($"- Daily Calories: {nutritionPlan.DailyCalorieTarget}");
            sb.AppendLine($"- Daily Protein: {nutritionPlan.DailyProteinTarget}g");
            sb.AppendLine($"- Daily Carbohydrates: {nutritionPlan.DailyCarbohydrateTarget}g");
            sb.AppendLine($"- Daily Fat: {nutritionPlan.DailyFatTarget}g");
            
            sb.AppendLine("\nPlease provide a 7-day meal plan with breakfast, lunch, dinner, and 2 snacks per day.");
            sb.AppendLine("Include specific food items, portion sizes, and nutritional information for each meal.");
            sb.AppendLine("Consider the user's dietary preferences and allergies.");
            
            return sb.ToString();
        }

        private string BuildRecipePrompt(List<FoodItem> ingredients)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Create a healthy recipe using the following ingredients:");
            
            foreach (var ingredient in ingredients)
            {
                sb.AppendLine($"- {ingredient.Name}: {ingredient.Quantity} {ingredient.Unit}");
            }
            
            sb.AppendLine("\nPlease provide:");
            sb.AppendLine("1. Recipe name");
            sb.AppendLine("2. Servings");
            sb.AppendLine("3. Ingredients list with quantities");
            sb.AppendLine("4. Step-by-step cooking instructions");
            sb.AppendLine("5. Nutritional information per serving");
            sb.AppendLine("6. Preparation time and cooking time");
            
            return sb.ToString();
        }

        private string BuildNutritionAdvicePrompt(UserProfile userProfile, string concern)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Provide nutrition advice for the following user:");
            sb.AppendLine($"User Profile:");
            sb.AppendLine($"- Age: {userProfile.Age}");
            sb.AppendLine($"- Gender: {userProfile.Gender}");
            sb.AppendLine($"- Height: {userProfile.Height} cm");
            sb.AppendLine($"- Weight: {userProfile.Weight} kg");
            sb.AppendLine($"- Activity Level: {userProfile.ActivityLevel?.Name}");
            sb.AppendLine($"- Dietary Preferences: {userProfile.DietaryPreferences}");
            sb.AppendLine($"- Allergies: {userProfile.Allergies}");
            sb.AppendLine($"- Health Conditions: {userProfile.ChronicConditions}");
            
            sb.AppendLine($"\nSpecific Concern: {concern}");
            sb.AppendLine("\nPlease provide evidence-based nutrition advice to address this concern.");
            sb.AppendLine("Include specific food recommendations, nutrients to focus on, and lifestyle suggestions.");
            
            return sb.ToString();
        }

        private async Task<string> CallGeminiAPIAsync(string prompt)
        {
            try
            {
                // Create the request
                var endpoint = EndpointName.FromProjectLocationPublisherModel(_projectId, _location, "google", _modelId);
                
                var instance = new Value
                {
                    StructValue = new Struct
                    {
                        Fields =
                        {
                            ["prompt"] = Value.ForString(prompt)
                        }
                    }
                };
                
                var parameters = new Value
                {
                    StructValue = new Struct
                    {
                        Fields =
                        {
                            ["temperature"] = Value.ForNumber(0.7),
                            ["maxOutputTokens"] = Value.ForNumber(1024)
                        }
                    }
                };
                
                var request = new PredictRequest
                {
                    EndpointAsEndpointName = endpoint,
                    Instances = { instance },
                    Parameters = parameters
                };
                
                // Make the prediction
                var response = await _predictionServiceClient.PredictAsync(request);
                
                // Extract the response text
                if (response.Predictions.Count > 0)
                {
                    var prediction = response.Predictions[0];
                    if (prediction.StructValue.Fields.TryGetValue("content", out var contentValue))
                    {
                        return contentValue.StringValue;
                    }
                }
                
                return "Unable to generate response from AI model.";
            }
            catch (Exception ex)
            {
                // Log the exception (in a real application, you would use a logging framework)
                Console.WriteLine($"Error calling Gemini API: {ex.Message}");
                return "Sorry, I'm currently unable to provide AI-generated advice. Please try again later.";
            }
        }
    }
}