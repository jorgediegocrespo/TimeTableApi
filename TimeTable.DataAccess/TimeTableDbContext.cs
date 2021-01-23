using Microsoft.EntityFrameworkCore;
using TimeTable.DataAccess.Contracts;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.EntityConfig;

namespace TimeTable.DataAccess
{
    public class TimeTableDbContext : DbContext, ITimeTableDbContext
    {
        public TimeTableDbContext(DbContextOptions options) : base(options) { }
        public TimeTableDbContext() { }

        public DbSet<BankDayEntity> BankDays { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<HolidayEntity> Holidays { get; set; }
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<VacationDayEntity> VacationDays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CompanyEntityConfig.SetEntityBuilder(modelBuilder.Entity<CompanyEntity>());
            PersonEntityConfig.SetEntityBuilder(modelBuilder.Entity<PersonEntity>());
            BankDayEntityConfig.SetEntityBuilder(modelBuilder.Entity<BankDayEntity>());            
            HolidayEntityConfig.SetEntityBuilder(modelBuilder.Entity<HolidayEntity>());            
            VacationDayEntityConfig.SetEntityBuilder(modelBuilder.Entity<VacationDayEntity>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
