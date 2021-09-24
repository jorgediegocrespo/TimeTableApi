using System;
using System.Threading;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts;

namespace TimeTable.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TimeTableDbContext timeTableDbContext;

        public UnitOfWork(TimeTableDbContext timeTableDbContext)
        {
            this.timeTableDbContext = timeTableDbContext;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return timeTableDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesInTransactionAsync(Func<Task<int>> operation)
        {
            using (var transaction = await timeTableDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    int result = await operation();
                    await transaction.CommitAsync();
                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
