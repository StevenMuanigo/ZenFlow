using ZenFlow.WorkoutService.DTOs;
using ZenFlow.WorkoutService.Models;

namespace ZenFlow.WorkoutService.Services
{
    public interface IWorkoutService
    {
        // Workout management
        Task<WorkoutDto> CreateWorkoutAsync(CreateWorkoutDto workoutDto);
        Task<WorkoutDto> GetWorkoutByIdAsync(string id);
        Task<IEnumerable<WorkoutDto>> GetWorkoutsByUserIdAsync(int userId);
        Task<WorkoutDto> UpdateWorkoutAsync(string id, WorkoutDto workoutDto);
        Task<bool> DeleteWorkoutAsync(string id);
        
        // Exercise management
        Task<ExerciseDto> CreateExerciseAsync(CreateExerciseDto exerciseDto);
        Task<ExerciseDto> GetExerciseByIdAsync(string id);
        Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync();
        Task<IEnumerable<ExerciseDto>> GetExercisesByMuscleGroupAsync(string muscleGroup);
        
        // Workout session management
        Task<WorkoutSessionDto> StartWorkoutSessionAsync(string workoutId, int userId);
        Task<WorkoutSessionDto> UpdateWorkoutSessionAsync(string sessionId, WorkoutSessionDto sessionDto);
        Task<WorkoutSessionDto> CompleteWorkoutSessionAsync(string sessionId);
        Task<WorkoutSessionDto> GetWorkoutSessionByIdAsync(string id);
        Task<IEnumerable<WorkoutSessionDto>> GetWorkoutSessionsByUserIdAsync(int userId);
    }
}