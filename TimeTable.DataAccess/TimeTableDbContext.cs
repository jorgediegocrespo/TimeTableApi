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

        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<TimeRecordEntity> TimeRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CompanyEntityConfig.SetEntityBuilder(modelBuilder.Entity<CompanyEntity>());
            PersonEntityConfig.SetEntityBuilder(modelBuilder.Entity<PersonEntity>());
            TimeRecordEntityConfig.SetEntityBuilder(modelBuilder.Entity<TimeRecordEntity>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
