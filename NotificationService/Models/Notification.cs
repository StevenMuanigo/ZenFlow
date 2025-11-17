using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ZenFlow.NotificationService.Models
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("userId")]
        public int UserId { get; set; }
        
        [BsonElement("title")]
        public string Title { get; set; }
        
        [BsonElement("message")]
        public string Message { get; set; }
        
        [BsonElement("type")]
        public string Type { get; set; } // Info, Warning, Success, Error
        
        [BsonElement("priority")]
        public string Priority { get; set; } // Low, Medium, High
        
        [BsonElement("isRead")]
        public bool IsRead { get; set; } = false;
        
        [BsonElement("relatedEntityId")]
        public string RelatedEntityId { get; set; }
        
        [BsonElement("relatedEntityType")]
        public string RelatedEntityType { get; set; } // Workout, Nutrition, Progress, etc.
        
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [BsonElement("readAt")]
        public DateTime? ReadAt { get; set; }
    }
}