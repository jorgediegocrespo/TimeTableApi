using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.EntityConfig
{
    public class TimeRecordEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<TimeRecordEntity> entityBuilder)
        {
            entityBuilder.ToTable("TimeRecords");

            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();

            entityBuilder.Property(x => x.StartDateTime).IsRequired(true);
            entityBuilder.Property(x => x.RowVersion).IsRequired(true);

            entityBuilder.HasOne(x => x.Person)
                .WithMany(x => x.TimeRecords)
                .IsRequired(true)
                .HasForeignKey(x => x.PersonId)
                .IsRequired(true);

            entityBuilder.HasQueryFilter(x => !x.IsDeleted);

            entityBuilder.HasIndex(x => x.IsDeleted);
            entityBuilder.HasIndex(x => x.PersonId);
        }
    }
}
