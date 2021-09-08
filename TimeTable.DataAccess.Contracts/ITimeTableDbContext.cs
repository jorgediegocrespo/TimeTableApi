using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.Contracts
{
    public interface ITimeTableDbContext
    {
        DbSet<CompanyEntity> Companies { get; set; }
        DbSet<PersonEntity> People { get; set; }
        DbSet<TimeRecordEntity> TimeRecords { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
