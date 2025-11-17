namespace ZenFlow.ReminderService.DTOs
{
    public class ReminderDto
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReminderType { get; set; }
        public DateTime ScheduledTime { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurrencePattern { get; set; }
        public bool IsActive { get; set; }
        public bool IsSent { get; set; }
        public string RelatedEntityId { get; set; }
        public string RelatedEntityType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}