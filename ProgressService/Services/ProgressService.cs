using MongoDB.Driver;
using MongoDB.Bson;
using ZenFlow.ProgressService.Data;
using ZenFlow.ProgressService.DTOs;
using ZenFlow.ProgressService.Models;

namespace ZenFlow.ProgressService.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressContext _context;

        public ProgressService(IProgressContext context)
        {
            _context = context;
        }

        public async Task<ProgressRecordDto> CreateProgressRecordAsync(CreateProgressRecordDto recordDto)
        {
            var progressRecord = new ProgressRecord
            {
                UserId = recordDto.UserId,
                Date = recordDto.Date,
                Weight = recordDto.Weight,
                BodyMeasurements = MapToBodyMeasurementsModel(recordDto.BodyMeasurements),
                FitnessMetrics = MapToFitnessMetricsModel(recordDto.FitnessMetrics),
                NutritionMetrics = MapToNutritionMetricsModel(recordDto.NutritionMetrics),
                WellnessMetrics = MapToWellnessMetricsModel(recordDto.WellnessMetrics),
                Notes = recordDto.Notes
            };

            await _context.ProgressRecords.InsertOneAsync(progressRecord);
            return MapToProgressRecordDto(progressRecord);
        }

        public async Task<ProgressRecordDto> GetProgressRecordByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid progress record ID format");
            }

            var record = await _context.ProgressRecords.Find(r => r.Id == id).FirstOrDefaultAsync();
            return record == null ? null : MapToProgressRecordDto(record);
        }

        public async Task<IEnumerable<ProgressRecordDto>> GetProgressRecordsByUserIdAsync(int userId)
        {
            var records = await _context.ProgressRecords
                .Find(r => r.UserId == userId)
                .SortByDescending(r => r.Date)
                .ToListAsync();
            
            return records.Select(r => MapToProgressRecordDto(r));
        }

        public async Task<IEnumerable<ProgressRecordDto>> GetProgressRecordsByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            var filter = Builders<ProgressRecord>.Filter.And(
                Builders<ProgressRecord>.Filter.Eq(r => r.UserId, userId),
                Builders<ProgressRecord>.Filter.Gte(r => r.Date, startDate),
                Builders<ProgressRecord>.Filter.Lte(r => r.Date, endDate)
            );

            var records = await _context.ProgressRecords
                .Find(filter)
                .SortBy(r => r.Date)
                .ToListAsync();
            
            return records.Select(r => MapToProgressRecordDto(r));
        }

        public async Task<ProgressRecordDto> UpdateProgressRecordAsync(string id, ProgressRecordDto recordDto)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid progress record ID format");
            }

            var update = Builders<ProgressRecord>.Update
                .Set(r => r.Date, recordDto.Date)
                .Set(r => r.Weight, recordDto.Weight)
                .Set(r => r.BodyMeasurements, MapToBodyMeasurementsModel(recordDto.BodyMeasurements))
                .Set(r => r.FitnessMetrics, MapToFitnessMetricsModel(recordDto.FitnessMetrics))
                .Set(r => r.NutritionMetrics, MapToNutritionMetricsModel(recordDto.NutritionMetrics))
                .Set(r => r.WellnessMetrics, MapToWellnessMetricsModel(recordDto.WellnessMetrics))
                .Set(r => r.Notes, recordDto.Notes)
                .Set(r => r.UpdatedAt, DateTime.UtcNow);

            var result = await _context.ProgressRecords.UpdateOneAsync(r => r.Id == id, update);
            if (result.ModifiedCount == 0)
            {
                return null;
            }

            var updatedRecord = await _context.ProgressRecords.Find(r => r.Id == id).FirstOrDefaultAsync();
            return MapToProgressRecordDto(updatedRecord);
        }

        public async Task<bool> DeleteProgressRecordAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                throw new ArgumentException("Invalid progress record ID format");
            }

            var result = await _context.ProgressRecords.DeleteOneAsync(r => r.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<ProgressSummaryDto> GetProgressSummaryAsync(int userId)
        {
            var records = await _context.ProgressRecords
                .Find(r => r.UserId == userId)
                .SortByDescending(r => r.Date)
                .ToListAsync();

            if (records.Count == 0)
            {
                return new ProgressSummaryDto();
            }

            var latestRecord = records.FirstOrDefault();
            var earliestRecord = records.LastOrDefault();
            
            // Calculate averages for wellness metrics
            var averageSleepHours = records.Average(r => r.WellnessMetrics.SleepHours);
            var averageSleepQuality = (int)records.Average(r => r.WellnessMetrics.SleepQuality);
            var averageStressLevel = (int)records.Average(r => r.WellnessMetrics.StressLevel);
            var averageMood = (int)records.Average(r => r.WellnessMetrics.Mood);
            
            // Sum up fitness metrics
            var totalWorkouts = records.Sum(r => r.FitnessMetrics.WorkoutsCompleted);
            var totalWorkoutTime = records.Sum(r => r.FitnessMetrics.TotalWorkoutTime);
            var totalCaloriesBurned = records.Sum(r => r.FitnessMetrics.CaloriesBurned);
            var totalSteps = records.Sum(r => r.FitnessMetrics.Steps);

            return new ProgressSummaryDto
            {
                TotalRecords = records.Count,
                CurrentWeight = latestRecord?.Weight,
                WeightChange = latestRecord?.Weight.HasValue == true && earliestRecord?.Weight.HasValue == true 
                    ? latestRecord.Weight - earliestRecord.Weight 
                    : null,
                CurrentBodyFatPercentage = latestRecord?.BodyMeasurements?.BodyFatPercentage,
                TotalWorkouts = totalWorkouts,
                TotalWorkoutTime = totalWorkoutTime,
                TotalCaloriesBurned = totalCaloriesBurned,
                TotalSteps = totalSteps,
                AverageSleepHours = averageSleepHours,
                AverageSleepQuality = averageSleepQuality,
                AverageStressLevel = averageStressLevel,
                AverageMood = averageMood,
                LastRecordDate = latestRecord?.Date ?? DateTime.MinValue
            };
        }

        public async Task<IEnumerable<ProgressChartDto>> GetProgressChartDataAsync(int userId, string metricType, DateTime startDate, DateTime endDate)
        {
            var records = await GetProgressRecordsByUserIdAndDateRangeAsync(userId, startDate, endDate);
            
            var chartData = new List<ProgressChartDto>();
            
            foreach (var record in records)
            {
                var chartPoint = new ProgressChartDto
                {
                    Date = record.Date,
                    MetricName = metricType
                };
                
                switch (metricType.ToLower())
                {
                    case "weight":
                        chartPoint.Value = record.Weight ?? 0;
                        break;
                    case "bodyfat":
                        chartPoint.Value = record.BodyMeasurements?.BodyFatPercentage ?? 0;
                        break;
                    case "sleephours":
                        chartPoint.Value = record.WellnessMetrics?.SleepHours ?? 0;
                        break;
                    case "sleepquality":
                        chartPoint.Value = record.WellnessMetrics?.SleepQuality ?? 0;
                        break;
                    case "stresslevel":
                        chartPoint.Value = record.WellnessMetrics?.StressLevel ?? 0;
                        break;
                    case "mood":
                        chartPoint.Value = record.WellnessMetrics?.Mood ?? 0;
                        break;
                    case "workouts":
                        chartPoint.Value = record.FitnessMetrics?.WorkoutsCompleted ?? 0;
                        break;
                    case "caloriesburned":
                        chartPoint.Value = record.FitnessMetrics?.CaloriesBurned ?? 0;
                        break;
                    default:
                        chartPoint.Value = 0;
                        break;
                }
                
                chartData.Add(chartPoint);
            }
            
            return chartData;
        }

        // Mapping methods
        private ProgressRecordDto MapToProgressRecordDto(ProgressRecord record)
        {
            return new ProgressRecordDto
            {
                Id = record.Id,
                UserId = record.UserId,
                Date = record.Date,
                Weight = record.Weight,
                BodyMeasurements = MapToBodyMeasurementsDto(record.BodyMeasurements),
                FitnessMetrics = MapToFitnessMetricsDto(record.FitnessMetrics),
                NutritionMetrics = MapToNutritionMetricsDto(record.NutritionMetrics),
                WellnessMetrics = MapToWellnessMetricsDto(record.WellnessMetrics),
                Notes = record.Notes
            };
        }

        private BodyMeasurementsDto MapToBodyMeasurementsDto(BodyMeasurements measurements)
        {
            if (measurements == null) return null;
            
            return new BodyMeasurementsDto
            {
                Height = measurements.Height,
                Chest = measurements.Chest,
                Waist = measurements.Waist,
                Hips = measurements.Hips,
                Arm = measurements.Arm,
                Thigh = measurements.Thigh,
                BodyFatPercentage = measurements.BodyFatPercentage
            };
        }

        private FitnessMetricsDto MapToFitnessMetricsDto(FitnessMetrics metrics)
        {
            if (metrics == null) return null;
            
            return new FitnessMetricsDto
            {
                WorkoutsCompleted = metrics.WorkoutsCompleted,
                TotalWorkoutTime = metrics.TotalWorkoutTime,
                CaloriesBurned = metrics.CaloriesBurned,
                Steps = metrics.Steps,
                Distance = metrics.Distance,
                ActiveMinutes = metrics.ActiveMinutes,
                RestingHeartRate = metrics.RestingHeartRate,
                MaxHeartRate = metrics.MaxHeartRate,
                Vo2Max = metrics.Vo2Max
            };
        }

        private NutritionMetricsDto MapToNutritionMetricsDto(NutritionMetrics metrics)
        {
            if (metrics == null) return null;
            
            return new NutritionMetricsDto
            {
                CaloriesConsumed = metrics.CaloriesConsumed,
                Protein = metrics.Protein,
                Carbohydrates = metrics.Carbohydrates,
                Fat = metrics.Fat,
                Fiber = metrics.Fiber,
                Water = metrics.Water,
                MealsLogged = metrics.MealsLogged,
                SupplementsTaken = metrics.SupplementsTaken
            };
        }

        private WellnessMetricsDto MapToWellnessMetricsDto(WellnessMetrics metrics)
        {
            if (metrics == null) return null;
            
            return new WellnessMetricsDto
            {
                SleepHours = metrics.SleepHours,
                SleepQuality = metrics.SleepQuality,
                StressLevel = metrics.StressLevel,
                Mood = metrics.Mood,
                EnergyLevel = metrics.EnergyLevel,
                HydrationLevel = metrics.HydrationLevel,
                Productivity = metrics.Productivity
            };
        }

        private BodyMeasurements MapToBodyMeasurementsModel(BodyMeasurementsDto measurementsDto)
        {
            if (measurementsDto == null) return null;
            
            return new BodyMeasurements
            {
                Height = measurementsDto.Height,
                Chest = measurementsDto.Chest,
                Waist = measurementsDto.Waist,
                Hips = measurementsDto.Hips,
                Arm = measurementsDto.Arm,
                Thigh = measurementsDto.Thigh,
                BodyFatPercentage = measurementsDto.BodyFatPercentage
            };
        }

        private FitnessMetrics MapToFitnessMetricsModel(FitnessMetricsDto metricsDto)
        {
            if (metricsDto == null) return null;
            
            return new FitnessMetrics
            {
                WorkoutsCompleted = metricsDto.WorkoutsCompleted,
                TotalWorkoutTime = metricsDto.TotalWorkoutTime,
                CaloriesBurned = metricsDto.CaloriesBurned,
                Steps = metricsDto.Steps,
                Distance = metricsDto.Distance,
                ActiveMinutes = metricsDto.ActiveMinutes,
                RestingHeartRate = metricsDto.RestingHeartRate,
                MaxHeartRate = metricsDto.MaxHeartRate,
                Vo2Max = metricsDto.Vo2Max
            };
        }

        private NutritionMetrics MapToNutritionMetricsModel(NutritionMetricsDto metricsDto)
        {
            if (metricsDto == null) return null;
            
            return new NutritionMetrics
            {
                CaloriesConsumed = metricsDto.CaloriesConsumed,
                Protein = metricsDto.Protein,
                Carbohydrates = metricsDto.Carbohydrates,
                Fat = metricsDto.Fat,
                Fiber = metricsDto.Fiber,
                Water = metricsDto.Water,
                MealsLogged = metricsDto.MealsLogged,
                SupplementsTaken = metricsDto.SupplementsTaken
            };
        }

        private WellnessMetrics MapToWellnessMetricsModel(WellnessMetricsDto metricsDto)
        {
            if (metricsDto == null) return null;
            
            return new WellnessMetrics
            {
                SleepHours = metricsDto.SleepHours,
                SleepQuality = metricsDto.SleepQuality,
                StressLevel = metricsDto.StressLevel,
                Mood = metricsDto.Mood,
                EnergyLevel = metricsDto.EnergyLevel,
                HydrationLevel = metricsDto.HydrationLevel,
                Productivity = metricsDto.Productivity
            };
        }
    }
}