using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.EntityConfig
{
    public class VacationDayEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<VacationDayEntity> entityBuilder)
        {
            entityBuilder.ToTable("VacationDays");

            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();

            entityBuilder.Property(x => x.Year).IsRequired(true);

            entityBuilder.HasIndex(a => a.Year).IsUnique(true);

            entityBuilder.HasOne(x => x.Person)
                .WithMany(x => x.VacationDays)
                .IsRequired(true)
                .HasForeignKey(x => x.PersonId)
                .IsRequired(true);
        }
    }
}
