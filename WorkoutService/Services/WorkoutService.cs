using MongoDB.Driver;
using MongoDB.Bson;
using ZenFlow.WorkoutService.Data;
using ZenFlow.WorkoutService.DTOs;
using ZenFlow.WorkoutService.Models;

namespace ZenFlow.WorkoutService.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutContext _context;

        public WorkoutService(IWorkoutContext context)
        {
            _context = context;
        }

        // Workout management
        public async Task<WorkoutDto> CreateWorkoutAsync(CreateWorkoutDto workoutDto)
        {
            var workout = new Workout
            {
                UserId = workoutDto.UserId,
                Name = workoutDto.Name,
                Description = workoutDto.Description,
                WorkoutType = workoutDto.WorkoutType,
                DifficultyLevel = workoutDto.DifficultyLevel,
                EstimatedDuration = workoutDto.EstimatedDuration,
                Exercises = workoutDto.Exercises.Select(e => MapToExerciseModel(e)).ToList()
            };

            await _context.Workouts.InsertOneAsync(workout);
            return MapToWorkoutDto(workout);
        }

        public async Task<WorkoutDto> GetWorkoutByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid workout ID format");
            }

            var workout = await _context.Workouts.Find(w => w.Id == id).FirstOrDefaultAsync();
            return workout == null ? null : MapToWorkoutDto(workout);
        }

        public async Task<IEnumerable<WorkoutDto>> GetWorkoutsByUserIdAsync(int userId)
        {
            var workouts = await _context.Workouts.Find(w => w.UserId == userId).ToListAsync();
            return workouts.Select(w => MapToWorkoutDto(w));
        }

        public async Task<WorkoutDto> UpdateWorkoutAsync(string id, WorkoutDto workoutDto)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid workout ID format");
            }

            var update = Builders<Workout>.Update
                .Set(w => w.Name, workoutDto.Name)
                .Set(w => w.Description, workoutDto.Description)
                .Set(w => w.WorkoutType, workoutDto.WorkoutType)
                .Set(w => w.DifficultyLevel, workoutDto.DifficultyLevel)
                .Set(w => w.EstimatedDuration, workoutDto.EstimatedDuration)
                .Set(w => w.Exercises, workoutDto.Exercises.Select(e => MapToExerciseModel(e)).ToList())
                .Set(w => w.UpdatedAt, DateTime.UtcNow);

            var result = await _context.Workouts.UpdateOneAsync(w => w.Id == id, update);
            if (result.ModifiedCount == 0)
            {
                return null;
            }

            var updatedWorkout = await _context.Workouts.Find(w => w.Id == id).FirstOrDefaultAsync();
            return MapToWorkoutDto(updatedWorkout);
        }

        public async Task<bool> DeleteWorkoutAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid workout ID format");
            }

            var result = await _context.Workouts.DeleteOneAsync(w => w.Id == id);
            return result.DeletedCount > 0;
        }

        // Exercise management
        public async Task<ExerciseDto> CreateExerciseAsync(CreateExerciseDto exerciseDto)
        {
            var exercise = new Exercise
            {
                Name = exerciseDto.Name,
                Description = exerciseDto.Description,
                MuscleGroup = exerciseDto.MuscleGroup,
                ExerciseType = exerciseDto.ExerciseType,
                Equipment = exerciseDto.Equipment,
                Instructions = exerciseDto.Instructions,
                VideoUrl = exerciseDto.VideoUrl,
                DefaultSets = exerciseDto.DefaultSets,
                DefaultReps = exerciseDto.DefaultReps,
                DefaultWeight = exerciseDto.DefaultWeight,
                DefaultDuration = exerciseDto.DefaultDuration
            };

            await _context.Exercises.InsertOneAsync(exercise);
            return MapToExerciseDto(exercise);
        }

        public async Task<ExerciseDto> GetExerciseByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid exercise ID format");
            }

            var exercise = await _context.Exercises.Find(e => e.Id == id).FirstOrDefaultAsync();
            return exercise == null ? null : MapToExerciseDto(exercise);
        }

        public async Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync()
        {
            var exercises = await _context.Exercises.Find(e => true).ToListAsync();
            return exercises.Select(e => MapToExerciseDto(e));
        }

        public async Task<IEnumerable<ExerciseDto>> GetExercisesByMuscleGroupAsync(string muscleGroup)
        {
            var exercises = await _context.Exercises.Find(e => e.MuscleGroup == muscleGroup).ToListAsync();
            return exercises.Select(e => MapToExerciseDto(e));
        }

        // Workout session management
        public async Task<WorkoutSessionDto> StartWorkoutSessionAsync(string workoutId, int userId)
        {
            if (!ObjectId.TryParse(workoutId, out _))
            {
                throw new ArgumentException("Invalid workout ID format");
            }

            // Get the workout to start
            var workout = await _context.Workouts.Find(w => w.Id == workoutId).FirstOrDefaultAsync();
            if (workout == null)
            {
                throw new ArgumentException("Workout not found");
            }

            // Create a new workout session
            var session = new WorkoutSession
            {
                UserId = userId,
                WorkoutId = workoutId,
                WorkoutName = workout.Name,
                StartDate = DateTime.UtcNow,
                Exercises = workout.Exercises.Select(e => new ExerciseLog
                {
                    ExerciseId = e.Id,
                    ExerciseName = e.Name,
                    Sets = Enumerable.Range(1, e.DefaultSets).Select(i => new SetLog
                    {
                        SetNumber = i,
                        Reps = e.DefaultReps,
                        Weight = e.DefaultWeight,
                        Duration = e.DefaultDuration,
                        Completed = false
                    }).ToList()
                }).ToList()
            };

            await _context.WorkoutSessions.InsertOneAsync(session);
            return MapToWorkoutSessionDto(session);
        }

        public async Task<WorkoutSessionDto> UpdateWorkoutSessionAsync(string sessionId, WorkoutSessionDto sessionDto)
        {
            if (!ObjectId.TryParse(sessionId, out _))
            {
                throw new ArgumentException("Invalid session ID format");
            }

            var update = Builders<WorkoutSession>.Update
                .Set(s => s.Exercises, sessionDto.Exercises.Select(e => MapToExerciseLogModel(e)).ToList())
                .Set(s => s.Notes, sessionDto.Notes)
                .Set(s => s.UpdatedAt, DateTime.UtcNow);

            var result = await _context.WorkoutSessions.UpdateOneAsync(s => s.Id == sessionId, update);
            if (result.ModifiedCount == 0)
            {
                return null;
            }

            var updatedSession = await _context.WorkoutSessions.Find(s => s.Id == sessionId).FirstOrDefaultAsync();
            return MapToWorkoutSessionDto(updatedSession);
        }

        public async Task<WorkoutSessionDto> CompleteWorkoutSessionAsync(string sessionId)
        {
            if (!ObjectId.TryParse(sessionId, out _))
            {
                throw new ArgumentException("Invalid session ID format");
            }

            var endDate = DateTime.UtcNow;
            
            var update = Builders<WorkoutSession>.Update
                .Set(s => s.EndDate, endDate)
                .Set(s => s.Duration, (int)(endDate - s.StartDate).TotalMinutes)
                .Set(s => s.Completed, true)
                .Set(s => s.UpdatedAt, DateTime.UtcNow);

            var result = await _context.WorkoutSessions.UpdateOneAsync(s => s.Id == sessionId, update);
            if (result.ModifiedCount == 0)
            {
                return null;
            }

            var updatedSession = await _context.WorkoutSessions.Find(s => s.Id == sessionId).FirstOrDefaultAsync();
            return MapToWorkoutSessionDto(updatedSession);
        }

        public async Task<WorkoutSessionDto> GetWorkoutSessionByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid session ID format");
            }

            var session = await _context.WorkoutSessions.Find(s => s.Id == id).FirstOrDefaultAsync();
            return session == null ? null : MapToWorkoutSessionDto(session);
        }

        public async Task<IEnumerable<WorkoutSessionDto>> GetWorkoutSessionsByUserIdAsync(int userId)
        {
            var sessions = await _context.WorkoutSessions.Find(s => s.UserId == userId).ToListAsync();
            return sessions.Select(s => MapToWorkoutSessionDto(s));
        }

        // Mapping methods
        private WorkoutDto MapToWorkoutDto(Workout workout)
        {
            return new WorkoutDto
            {
                Id = workout.Id,
                UserId = workout.UserId,
                Name = workout.Name,
                Description = workout.Description,
                WorkoutType = workout.WorkoutType,
                DifficultyLevel = workout.DifficultyLevel,
                EstimatedDuration = workout.EstimatedDuration,
                Exercises = workout.Exercises.Select(e => MapToExerciseDto(e)).ToList()
            };
        }

        private ExerciseDto MapToExerciseDto(Exercise exercise)
        {
            return new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                MuscleGroup = exercise.MuscleGroup,
                ExerciseType = exercise.ExerciseType,
                Equipment = exercise.Equipment,
                Instructions = exercise.Instructions,
                VideoUrl = exercise.VideoUrl,
                DefaultSets = exercise.DefaultSets,
                DefaultReps = exercise.DefaultReps,
                DefaultWeight = exercise.DefaultWeight,
                DefaultDuration = exercise.DefaultDuration
            };
        }

        private Exercise MapToExerciseModel(CreateExerciseDto exerciseDto)
        {
            return new Exercise
            {
                Name = exerciseDto.Name,
                Description = exerciseDto.Description,
                MuscleGroup = exerciseDto.MuscleGroup,
                ExerciseType = exerciseDto.ExerciseType,
                Equipment = exerciseDto.Equipment,
                Instructions = exerciseDto.Instructions,
                VideoUrl = exerciseDto.VideoUrl,
                DefaultSets = exerciseDto.DefaultSets,
                DefaultReps = exerciseDto.DefaultReps,
                DefaultWeight = exerciseDto.DefaultWeight,
                DefaultDuration = exerciseDto.DefaultDuration
            };
        }

        private WorkoutSessionDto MapToWorkoutSessionDto(WorkoutSession session)
        {
            return new WorkoutSessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                WorkoutId = session.WorkoutId,
                WorkoutName = session.WorkoutName,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                Duration = session.Duration,
                Exercises = session.Exercises.Select(e => MapToExerciseLogDto(e)).ToList(),
                CaloriesBurned = session.CaloriesBurned,
                Notes = session.Notes,
                Completed = session.Completed
            };
        }

        private ExerciseLogDto MapToExerciseLogDto(ExerciseLog exerciseLog)
        {
            return new ExerciseLogDto
            {
                Id = exerciseLog.Id,
                ExerciseId = exerciseLog.ExerciseId,
                ExerciseName = exerciseLog.ExerciseName,
                Sets = exerciseLog.Sets.Select(s => MapToSetLogDto(s)).ToList(),
                Notes = exerciseLog.Notes
            };
        }

        private SetLogDto MapToSetLogDto(SetLog setLog)
        {
            return new SetLogDto
            {
                Id = setLog.Id,
                SetNumber = setLog.SetNumber,
                Reps = setLog.Reps,
                Weight = setLog.Weight,
                Duration = setLog.Duration,
                Completed = setLog.Completed,
                Notes = setLog.Notes
            };
        }

        private ExerciseLog MapToExerciseLogModel(ExerciseLogDto exerciseLogDto)
        {
            return new ExerciseLog
            {
                Id = exerciseLogDto.Id,
                ExerciseId = exerciseLogDto.ExerciseId,
                ExerciseName = exerciseLogDto.ExerciseName,
                Sets = exerciseLogDto.Sets.Select(s => MapToSetLogModel(s)).ToList(),
                Notes = exerciseLogDto.Notes
            };
        }

        private SetLog MapToSetLogModel(SetLogDto setLogDto)
        {
            return new SetLog
            {
                Id = setLogDto.Id,
                SetNumber = setLogDto.SetNumber,
                Reps = setLogDto.Reps,
                Weight = setLogDto.Weight,
                Duration = setLogDto.Duration,
                Completed = setLogDto.Completed,
                Notes = setLogDto.Notes
            };
        }
    }
}