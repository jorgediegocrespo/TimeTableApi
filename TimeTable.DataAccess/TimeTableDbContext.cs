using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.EntityConfig;

namespace TimeTable.DataAccess
{
    public class TimeTableDbContext : IdentityDbContext
    {
        public TimeTableDbContext(DbContextOptions options) : base(options) { }
        public TimeTableDbContext() { }

        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<PersonEntity> People { get; set; }
        public DbSet<TimeRecordEntity> TimeRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            CompanyEntityConfig.SetEntityBuilder(modelBuilder.Entity<CompanyEntity>());
            PersonEntityConfig.SetEntityBuilder(modelBuilder.Entity<PersonEntity>());
            TimeRecordEntityConfig.SetEntityBuilder(modelBuilder.Entity<TimeRecordEntity>());
        }
    }
}
