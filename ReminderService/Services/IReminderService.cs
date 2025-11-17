using ZenFlow.ReminderService.DTOs;
using ZenFlow.ReminderService.Models;

namespace ZenFlow.ReminderService.Services
{
    public interface IReminderService
    {
        Task<ReminderDto> CreateReminderAsync(CreateReminderDto reminderDto);
        Task<ReminderDto> GetReminderByIdAsync(string id);
        Task<IEnumerable<ReminderDto>> GetRemindersByUserIdAsync(int userId);
        Task<IEnumerable<ReminderDto>> GetActiveRemindersByUserIdAsync(int userId);
        Task<ReminderDto> UpdateReminderAsync(string id, ReminderDto reminderDto);
        Task<bool> DeleteReminderAsync(string id);
        Task<IEnumerable<ReminderDto>> GetDueRemindersAsync();
        Task<bool> MarkReminderAsSentAsync(string id);
    }
}