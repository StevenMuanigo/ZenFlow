using MongoDB.Driver;
using ZenFlow.NotificationService.Models;

namespace ZenFlow.NotificationService.Data
{
    public interface INotificationContext
    {
        IMongoCollection<Notification> Notifications { get; }
    }
}