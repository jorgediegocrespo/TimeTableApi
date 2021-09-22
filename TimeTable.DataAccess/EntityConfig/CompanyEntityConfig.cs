﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.EntityConfig
{
    public class CompanyEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<CompanyEntity> entityBuilder)
        {
            entityBuilder.ToTable("Companies");

            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();

            entityBuilder.Property(x => x.Name).IsRequired(true);

            entityBuilder.HasIndex(a => a.Name).IsUnique(true);

            entityBuilder.HasMany(x => x.People).WithOne(x => x.Company);
        }
    }
}