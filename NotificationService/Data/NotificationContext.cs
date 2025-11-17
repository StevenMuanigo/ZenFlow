using MongoDB.Driver;
using ZenFlow.NotificationService.Models;

namespace ZenFlow.NotificationService.Data
{
    public class NotificationContext : INotificationContext
    {
        public NotificationContext(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("ZenFlowNotificationDb");
            Notifications = database.GetCollection<Notification>("Notifications");
        }

        public IMongoCollection<Notification> Notifications { get; }
    }
}