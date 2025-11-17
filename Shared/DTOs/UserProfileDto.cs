namespace ZenFlow.Shared.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string Allergies { get; set; }
        public string ChronicConditions { get; set; }
        public string DietaryPreferences { get; set; }
        public int ActivityLevelId { get; set; }
        public string ActivityLevelName { get; set; }
        public ICollection<int> HealthGoalIds { get; set; } = new List<int>();
        public ICollection<string> HealthGoalNames { get; set; } = new List<string>();
    }
}