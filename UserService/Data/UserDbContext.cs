using Microsoft.EntityFrameworkCore;
using ZenFlow.UserService.Models;

namespace ZenFlow.UserService
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<HealthGoal> HealthGoals { get; set; }
        public DbSet<ActivityLevel> ActivityLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(d => d.User)
                    .WithOne(p => p.Profile)
                    .HasForeignKey<UserProfile>(d => d.UserId);
            });

            modelBuilder.Entity<HealthGoal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ActivityLevel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            // Seed data for activity levels
            modelBuilder.Entity<ActivityLevel>().HasData(
                new ActivityLevel { Id = 1, Name = "Sedentary", Description = "Little or no exercise" },
                new ActivityLevel { Id = 2, Name = "Lightly Active", Description = "Light exercise 1-3 days/week" },
                new ActivityLevel { Id = 3, Name = "Moderately Active", Description = "Moderate exercise 3-5 days/week" },
                new ActivityLevel { Id = 4, Name = "Very Active", Description = "Hard exercise 6-7 days/week" },
                new ActivityLevel { Id = 5, Name = "Extra Active", Description = "Very hard exercise & physical job" }
            );

            // Seed data for health goals
            modelBuilder.Entity<HealthGoal>().HasData(
                new HealthGoal { Id = 1, Name = "Weight Loss", Description = "Lose weight in a healthy way" },
                new HealthGoal { Id = 2, Name = "Muscle Gain", Description = "Build muscle mass and strength" },
                new HealthGoal { Id = 3, Name = "Energy Boost", Description = "Increase energy levels throughout the day" },
                new HealthGoal { Id = 4, Name = "Stress Reduction", Description = "Reduce stress and improve mental well-being" },
                new HealthGoal { Id = 5, Name = "Better Sleep", Description = "Improve sleep quality and duration" }
            );
        }
    }
}