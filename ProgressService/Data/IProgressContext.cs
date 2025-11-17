using MongoDB.Driver;
using ZenFlow.ProgressService.Models;

namespace ZenFlow.ProgressService.Data
{
    public interface IProgressContext
    {
        IMongoCollection<ProgressRecord> ProgressRecords { get; }
    }
}