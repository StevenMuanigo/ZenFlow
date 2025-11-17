using MongoDB.Driver;
using MongoDB.Bson;
using ZenFlow.NotificationService.Data;
using ZenFlow.NotificationService.DTOs;
using ZenFlow.NotificationService.Models;

namespace ZenFlow.NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationContext _context;

        public NotificationService(INotificationContext context)
        {
            _context = context;
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto notificationDto)
        {
            var notification = new Notification
            {
                UserId = notificationDto.UserId,
                Title = notificationDto.Title,
                Message = notificationDto.Message,
                Type = notificationDto.Type,
                Priority = notificationDto.Priority,
                RelatedEntityId = notificationDto.RelatedEntityId,
                RelatedEntityType = notificationDto.RelatedEntityType
            };

            await _context.Notifications.InsertOneAsync(notification);
            return MapToNotificationDto(notification);
        }

        public async Task<NotificationDto> GetNotificationByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid notification ID format");
            }

            var notification = await _context.Notifications.Find(n => n.Id == id).FirstOrDefaultAsync();
            return notification == null ? null : MapToNotificationDto(notification);
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Find(n => n.UserId == userId)
                .SortByDescending(n => n.CreatedAt)
                .ToListAsync();
            
            return notifications.Select(n => MapToNotificationDto(n));
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserIdAsync(int userId)
        {
            var filter = Builders<Notification>.Filter.And(
                Builders<Notification>.Filter.Eq(n => n.UserId, userId),
                Builders<Notification>.Filter.Eq(n => n.IsRead, false)
            );

            var notifications = await _context.Notifications
                .Find(filter)
                .SortByDescending(n => n.CreatedAt)
                .ToListAsync();
            
            return notifications.Select(n => MapToNotificationDto(n));
        }

        public async Task<NotificationDto> MarkNotificationAsReadAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid notification ID format");
            }

            var update = Builders<Notification>.Update
                .Set(n => n.IsRead, true)
                .Set(n => n.ReadAt, DateTime.UtcNow);

            var result = await _context.Notifications.UpdateOneAsync(n => n.Id == id, update);
            if (result.ModifiedCount == 0)
            {
                return null;
            }

            var updatedNotification = await _context.Notifications.Find(n => n.Id == id).FirstOrDefaultAsync();
            return MapToNotificationDto(updatedNotification);
        }

        public async Task<IEnumerable<NotificationDto>> MarkAllNotificationsAsReadAsync(int userId)
        {
            var filter = Builders<Notification>.Filter.And(
                Builders<Notification>.Filter.Eq(n => n.UserId, userId),
                Builders<Notification>.Filter.Eq(n => n.IsRead, false)
            );

            var update = Builders<Notification>.Update
                .Set(n => n.IsRead, true)
                .Set(n => n.ReadAt, DateTime.UtcNow);

            await _context.Notifications.UpdateManyAsync(filter, update);

            // Return the updated notifications
            var updatedNotifications = await _context.Notifications
                .Find(filter)
                .SortByDescending(n => n.CreatedAt)
                .ToListAsync();
            
            return updatedNotifications.Select(n => MapToNotificationDto(n));
        }

        public async Task<bool> DeleteNotificationAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid notification ID format");
            }

            var result = await _context.Notifications.DeleteOneAsync(n => n.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<int> GetUnreadNotificationCountAsync(int userId)
        {
            var filter = Builders<Notification>.Filter.And(
                Builders<Notification>.Filter.Eq(n => n.UserId, userId),
                Builders<Notification>.Filter.Eq(n => n.IsRead, false)
            );

            return (int)await _context.Notifications.CountDocumentsAsync(filter);
        }

        // Mapping method
        private NotificationDto MapToNotificationDto(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                Priority = notification.Priority,
                IsRead = notification.IsRead,
                RelatedEntityId = notification.RelatedEntityId,
                RelatedEntityType = notification.RelatedEntityType,
                CreatedAt = notification.CreatedAt,
                ReadAt = notification.ReadAt
            };
        }
    }
}