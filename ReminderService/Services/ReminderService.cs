using MongoDB.Driver;
using MongoDB.Bson;
using ZenFlow.ReminderService.Data;
using ZenFlow.ReminderService.DTOs;
using ZenFlow.ReminderService.Models;

namespace ZenFlow.ReminderService.Services
{
    public class ReminderService : IReminderService
    {
        private readonly IReminderContext _context;

        public ReminderService(IReminderContext context)
        {
            _context = context;
        }

        public async Task<ReminderDto> CreateReminderAsync(CreateReminderDto reminderDto)
        {
            var reminder = new Reminder
            {
                UserId = reminderDto.UserId,
                Title = reminderDto.Title,
                Description = reminderDto.Description,
                ReminderType = reminderDto.ReminderType,
                ScheduledTime = reminderDto.ScheduledTime,
                IsRecurring = reminderDto.IsRecurring,
                RecurrencePattern = reminderDto.RecurrencePattern,
                RelatedEntityId = reminderDto.RelatedEntityId,
                RelatedEntityType = reminderDto.RelatedEntityType
            };

            await _context.Reminders.InsertOneAsync(reminder);
            return MapToReminderDto(reminder);
        }

        public async Task<ReminderDto> GetReminderByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid reminder ID format");
            }

            var reminder = await _context.Reminders.Find(r => r.Id == id).FirstOrDefaultAsync();
            return reminder == null ? null : MapToReminderDto(reminder);
        }

        public async Task<IEnumerable<ReminderDto>> GetRemindersByUserIdAsync(int userId)
        {
            var reminders = await _context.Reminders
                .Find(r => r.UserId == userId)
                .SortByDescending(r => r.ScheduledTime)
                .ToListAsync();
            
            return reminders.Select(r => MapToReminderDto(r));
        }

        public async Task<IEnumerable<ReminderDto>> GetActiveRemindersByUserIdAsync(int userId)
        {
            var filter = Builders<Reminder>.Filter.And(
                Builders<Reminder>.Filter.Eq(r => r.UserId, userId),
                Builders<Reminder>.Filter.Eq(r => r.IsActive, true)
            );

            var reminders = await _context.Reminders
                .Find(filter)
                .SortByDescending(r => r.ScheduledTime)
                .ToListAsync();
            
            return reminders.Select(r => MapToReminderDto(r));
        }

        public async Task<ReminderDto> UpdateReminderAsync(string id, ReminderDto reminderDto)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid reminder ID format");
            }

            var update = Builders<Reminder>.Update
                .Set(r => r.Title, reminderDto.Title)
                .Set(r => r.Description, reminderDto.Description)
                .Set(r => r.ReminderType, reminderDto.ReminderType)
                .Set(r => r.ScheduledTime, reminderDto.ScheduledTime)
                .Set(r => r.IsRecurring, reminderDto.IsRecurring)
                .Set(r => r.RecurrencePattern, reminderDto.RecurrencePattern)
                .Set(r => r.IsActive, reminderDto.IsActive)
                .Set(r => r.RelatedEntityId, reminderDto.RelatedEntityId)
                .Set(r => r.RelatedEntityType, reminderDto.RelatedEntityType)
                .Set(r => r.UpdatedAt, DateTime.UtcNow);

            var result = await _context.Reminders.UpdateOneAsync(r => r.Id == id, update);
            if (result.ModifiedCount == 0)
            {
                return null;
            }

            var updatedReminder = await _context.Reminders.Find(r => r.Id == id).FirstOrDefaultAsync();
            return MapToReminderDto(updatedReminder);
        }

        public async Task<bool> DeleteReminderAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid reminder ID format");
            }

            var result = await _context.Reminders.DeleteOneAsync(r => r.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<ReminderDto>> GetDueRemindersAsync()
        {
            var filter = Builders<Reminder>.Filter.And(
                Builders<Reminder>.Filter.Lte(r => r.ScheduledTime, DateTime.UtcNow),
                Builders<Reminder>.Filter.Eq(r => r.IsActive, true),
                Builders<Reminder>.Filter.Eq(r => r.IsSent, false)
            );

            var reminders = await _context.Reminders.Find(filter).ToListAsync();
            return reminders.Select(r => MapToReminderDto(r));
        }

        public async Task<bool> MarkReminderAsSentAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid reminder ID format");
            }

            var update = Builders<Reminder>.Update
                .Set(r => r.IsSent, true)
                .Set(r => r.UpdatedAt, DateTime.UtcNow);

            var result = await _context.Reminders.UpdateOneAsync(r => r.Id == id, update);
            return result.ModifiedCount > 0;
        }

        // Mapping method
        private ReminderDto MapToReminderDto(Reminder reminder)
        {
            return new ReminderDto
            {
                Id = reminder.Id,
                UserId = reminder.UserId,
                Title = reminder.Title,
                Description = reminder.Description,
                ReminderType = reminder.ReminderType,
                ScheduledTime = reminder.ScheduledTime,
                IsRecurring = reminder.IsRecurring,
                RecurrencePattern = reminder.RecurrencePattern,
                IsActive = reminder.IsActive,
                IsSent = reminder.IsSent,
                RelatedEntityId = reminder.RelatedEntityId,
                RelatedEntityType = reminder.RelatedEntityType,
                CreatedAt = reminder.CreatedAt,
                UpdatedAt = reminder.UpdatedAt
            };
        }
    }
}