using ZenFlow.NotificationService.DTOs;
using ZenFlow.NotificationService.Models;

namespace ZenFlow.NotificationService.Services
{
    public interface INotificationService
    {
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto notificationDto);
        Task<NotificationDto> GetNotificationByIdAsync(string id);
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId);
        Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserIdAsync(int userId);
        Task<NotificationDto> MarkNotificationAsReadAsync(string id);
        Task<IEnumerable<NotificationDto>> MarkAllNotificationsAsReadAsync(int userId);
        Task<bool> DeleteNotificationAsync(string id);
        Task<int> GetUnreadNotificationCountAsync(int userId);
    }
}