using MongoDB.Driver;
using ZenFlow.ProgressService.Models;

namespace ZenFlow.ProgressService.Data
{
    public class ProgressContext : IProgressContext
    {
        public ProgressContext(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("ZenFlowProgressDb");
            ProgressRecords = database.GetCollection<ProgressRecord>("ProgressRecords");
        }

        public IMongoCollection<ProgressRecord> ProgressRecords { get; }
    }
}