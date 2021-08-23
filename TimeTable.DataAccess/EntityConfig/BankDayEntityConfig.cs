using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.EntityConfig
{
    public class BankDayEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<BankDayEntity> entityBuilder)
        {
            entityBuilder.ToTable("BankDays");

            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();

            entityBuilder.Property(x => x.Day).IsRequired(true);

            entityBuilder.HasOne(x => x.Company)
                .WithMany(x => x.BankDays)
                .IsRequired(true)
                .HasForeignKey(x => x.CompanyId)
                .IsRequired(true);
        }
    }
}
