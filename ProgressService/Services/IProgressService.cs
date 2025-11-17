using ZenFlow.ProgressService.DTOs;
using ZenFlow.ProgressService.Models;

namespace ZenFlow.ProgressService.Services
{
    public interface IProgressService
    {
        Task<ProgressRecordDto> CreateProgressRecordAsync(CreateProgressRecordDto recordDto);
        Task<ProgressRecordDto> GetProgressRecordByIdAsync(string id);
        Task<IEnumerable<ProgressRecordDto>> GetProgressRecordsByUserIdAsync(int userId);
        Task<IEnumerable<ProgressRecordDto>> GetProgressRecordsByUserIdAndDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<ProgressRecordDto> UpdateProgressRecordAsync(string id, ProgressRecordDto recordDto);
        Task<bool> DeleteProgressRecordAsync(string id);
        
        Task<ProgressSummaryDto> GetProgressSummaryAsync(int userId);
        Task<IEnumerable<ProgressChartDto>> GetProgressChartDataAsync(int userId, string metricType, DateTime startDate, DateTime endDate);
    }
}