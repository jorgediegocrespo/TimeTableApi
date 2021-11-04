using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.EntityConfig
{
    public class PersonEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<PersonEntity> entityBuilder)
        {
            entityBuilder.ToTable("People");

            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();

            entityBuilder.Property(x => x.Name).IsRequired(true);
            entityBuilder.Property(x => x.IsAdmin).IsRequired(true);

            entityBuilder.HasOne(x => x.Company)
                .WithMany(x => x.People)
                .IsRequired(true)
                .HasForeignKey(x => x.CompanyId)
                .IsRequired(true);

            entityBuilder.HasMany(x => x.TimeRecords).WithOne(x => x.Person);
        }
    }
}
