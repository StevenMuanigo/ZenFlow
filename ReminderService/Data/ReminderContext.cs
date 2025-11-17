using MongoDB.Driver;
using ZenFlow.ReminderService.Models;

namespace ZenFlow.ReminderService.Data
{
    public class ReminderContext : IReminderContext
    {
        public ReminderContext(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("ZenFlowReminderDb");
            Reminders = database.GetCollection<Reminder>("Reminders");
        }

        public IMongoCollection<Reminder> Reminders { get; }
    }
}