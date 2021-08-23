﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.DataAccess.EntityConfig
{
    public class HolidayEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<HolidayEntity> entityBuilder)
        {
            entityBuilder.ToTable("Holidays");

            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).ValueGeneratedOnAdd();

            entityBuilder.Property(x => x.Date).IsRequired(true);
            entityBuilder.Property(x => x.Confirmed).IsRequired(true);

            entityBuilder.HasOne(x => x.PersonRequesting)
                .WithMany(x => x.HolidaysRequested)
                .IsRequired(true)
                .HasForeignKey(x => x.PersonRequestingId)
                .IsRequired(true);

            entityBuilder.HasOne(x => x.ConfirmingPerson)
                .WithMany(x => x.ConfirmedHolidays)
                .IsRequired(false)
                .HasForeignKey(x => x.ConfirmingPersonId)
                .IsRequired(false);
        }
    }
}
