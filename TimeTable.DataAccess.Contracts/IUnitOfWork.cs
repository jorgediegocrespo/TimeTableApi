using System;
using System.Threading;
using System.Threading.Tasks;

namespace TimeTable.DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        void ClearTracker();
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public Task<int> SaveChangesInTransactionAsync(Func<Task<int>> operation);
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }
}
