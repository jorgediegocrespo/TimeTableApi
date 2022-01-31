using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;
using TimeTable.DataAccess.Contracts.Entities;
using TimeTable.DataAccess.Contracts.Entities.Base;
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

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (EntityEntry entry in ChangeTracker.Entries())
            {
                BaseEntity baseEntity = entry.Entity as BaseEntity;
                if (baseEntity == null)
                    return;

                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.IsDeleted = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        baseEntity.IsDeleted = true;
                        break;
                }
            }
        }
    }
}
