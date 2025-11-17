using MongoDB.Driver;
using ZenFlow.WorkoutService.Models;

namespace ZenFlow.WorkoutService.Data
{
    public interface IWorkoutContext
    {
        IMongoCollection<Workout> Workouts { get; }
        IMongoCollection<Exercise> Exercises { get; }
        IMongoCollection<WorkoutSession> WorkoutSessions { get; }
    }
}