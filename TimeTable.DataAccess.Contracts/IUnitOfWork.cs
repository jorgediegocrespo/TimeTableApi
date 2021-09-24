using System;
using System.Threading;
using System.Threading.Tasks;

namespace TimeTable.DataAccess.Contracts
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public Task<int> SaveChangesInTransactionAsync(Func<Task<int>> operation);
    }
}
