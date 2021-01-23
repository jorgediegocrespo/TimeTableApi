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
        DbSet<BankDayEntity> BankDays { get; set; }        
        DbSet<HolidayEntity> Holidays { get; set; }        
        DbSet<VacationDayEntity> VacationDays { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
