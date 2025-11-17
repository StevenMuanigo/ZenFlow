using MongoDB.Driver;
using ZenFlow.WorkoutService.Models;

namespace ZenFlow.WorkoutService.Data
{
    public class WorkoutContext : IWorkoutContext
    {
        public WorkoutContext(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("ZenFlowWorkoutDb");
            Workouts = database.GetCollection<Workout>("Workouts");
            Exercises = database.GetCollection<Exercise>("Exercises");
            WorkoutSessions = database.GetCollection<WorkoutSession>("WorkoutSessions");
        }

        public IMongoCollection<Workout> Workouts { get; }
        public IMongoCollection<Exercise> Exercises { get; }
        public IMongoCollection<WorkoutSession> WorkoutSessions { get; }
    }
}