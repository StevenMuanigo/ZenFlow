using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.ReminderService.Models
{
    public class Reminder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("userId")]
        public int UserId { get; set; }
        
        [BsonElement("title")]
        public string Title { get; set; }
        
        [BsonElement("description")]
        public string Description { get; set; }
        
        [BsonElement("reminderType")]
        public string ReminderType { get; set; } // Meal, Workout, Water, Medicine, etc.
        
        [BsonElement("scheduledTime")]
        public DateTime ScheduledTime { get; set; }
        
        [BsonElement("isRecurring")]
        public bool IsRecurring { get; set; }
        
        [BsonElement("recurrencePattern")]
        public string RecurrencePattern { get; set; } // Daily, Weekly, Monthly, etc.
        
        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
        
        [BsonElement("isSent")]
        public bool IsSent { get; set; } = false;
        
        [BsonElement("relatedEntityId")]
        public string RelatedEntityId { get; set; }
        
        [BsonElement("relatedEntityType")]
        public string RelatedEntityType { get; set; } // Workout, Nutrition, etc.
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}