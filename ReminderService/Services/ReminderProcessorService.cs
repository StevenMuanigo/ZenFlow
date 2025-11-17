using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ZenFlow.ReminderService.Data;
using ZenFlow.ReminderService.Models;

namespace ZenFlow.ReminderService.Services
{
    public class ReminderProcessorService : IHostedService, IDisposable
    {
        private readonly ILogger<ReminderProcessorService> _logger;
        private readonly IReminderContext _reminderContext;
        private Timer _timer;

        public ReminderProcessorService(
            ILogger<ReminderProcessorService> logger,
            IReminderContext reminderContext)
        {
            _logger = logger;
            _reminderContext = reminderContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder Processor Service is starting.");
            
            // Run every minute
            _timer = new Timer(ProcessReminders, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reminder Processor Service is stopping.");
            
            _timer?.Change(Timeout.Infinite, 0);
            
            return Task.CompletedTask;
        }

        private async void ProcessReminders(object state)
        {
            try
            {
                _logger.LogInformation("Processing due reminders at {Time}", DateTime.UtcNow);
                
                // Get due reminders
                var dueReminders = await GetDueRemindersAsync();
                
                foreach (var reminder in dueReminders)
                {
                    // Send notification (in a real app, this would integrate with a notification service)
                    await SendNotificationAsync(reminder);
                    
                    // Mark reminder as sent
                    await MarkReminderAsSentAsync(reminder.Id);
                    
                    // If it's a recurring reminder, create a new one for the next occurrence
                    if (reminder.IsRecurring)
                    {
                        await CreateNextOccurrenceAsync(reminder);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing reminders");
            }
        }

        private async Task<IEnumerable<Reminder>> GetDueRemindersAsync()
        {
            var filter = Builders<Reminder>.Filter.And(
                Builders<Reminder>.Filter.Lte(r => r.ScheduledTime, DateTime.UtcNow),
                Builders<Reminder>.Filter.Eq(r => r.IsActive, true),
                Builders<Reminder>.Filter.Eq(r => r.IsSent, false)
            );

            return await _reminderContext.Reminders.Find(filter).ToListAsync();
        }

        private async Task SendNotificationAsync(Reminder reminder)
        {
            // In a real application, this would integrate with a notification service
            // to send push notifications, emails, or SMS messages
            _logger.LogInformation("Sending reminder notification to user {UserId}: {Title}", 
                reminder.UserId, reminder.Title);
            
            // Simulate sending notification
            await Task.Delay(100);
        }

        private async Task MarkReminderAsSentAsync(string reminderId)
        {
            var update = Builders<Reminder>.Update
                .Set(r => r.IsSent, true)
                .Set(r => r.UpdatedAt, DateTime.UtcNow);

            await _reminderContext.Reminders.UpdateOneAsync(
                r => r.Id == reminderId, 
                update);
        }

        private async Task CreateNextOccurrenceAsync(Reminder reminder)
        {
            var nextScheduledTime = CalculateNextOccurrence(reminder.ScheduledTime, reminder.RecurrencePattern);
            
            var newReminder = new Reminder
            {
                UserId = reminder.UserId,
                Title = reminder.Title,
                Description = reminder.Description,
                ReminderType = reminder.ReminderType,
                ScheduledTime = nextScheduledTime,
                IsRecurring = reminder.IsRecurring,
                RecurrencePattern = reminder.RecurrencePattern,
                IsActive = reminder.IsActive,
                IsSent = false,
                RelatedEntityId = reminder.RelatedEntityId,
                RelatedEntityType = reminder.RelatedEntityType
            };

            await _reminderContext.Reminders.InsertOneAsync(newReminder);
            _logger.LogInformation("Created next occurrence of recurring reminder: {Title}", reminder.Title);
        }

        private DateTime CalculateNextOccurrence(DateTime scheduledTime, string recurrencePattern)
        {
            return recurrencePattern.ToLower() switch
            {
                "daily" => scheduledTime.AddDays(1),
                "weekly" => scheduledTime.AddDays(7),
                "monthly" => scheduledTime.AddMonths(1),
                _ => scheduledTime.AddDays(1) // Default to daily
            };
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}