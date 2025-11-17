namespace ZenFlow.ReminderService.DTOs
{
    public class CreateReminderDto
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ReminderType { get; set; }
        public DateTime ScheduledTime { get; set; }
        public bool IsRecurring { get; set; }
        public string RecurrencePattern { get; set; }
        public string RelatedEntityId { get; set; }
        public string RelatedEntityType { get; set; }
    }
}