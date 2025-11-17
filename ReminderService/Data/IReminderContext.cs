using MongoDB.Driver;
using ZenFlow.ReminderService.Models;

namespace ZenFlow.ReminderService.Data
{
    public interface IReminderContext
    {
        IMongoCollection<Reminder> Reminders { get; }
    }
}