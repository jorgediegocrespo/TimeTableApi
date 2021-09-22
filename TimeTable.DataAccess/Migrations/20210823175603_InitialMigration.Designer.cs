﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeTable.DataAccess;

namespace TimeTable.DataAccess.Migrations
{
    [DbContext(typeof(TimeTableDbContext))]
    [Migration("20210823175603_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TimeTable.DataAccess.Contracts.Entities.CompanyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("TimeTable.DataAccess.Contracts.Entities.PersonEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("People");
                });

            modelBuilder.Entity("TimeTable.DataAccess.Contracts.Entities.TimeRecordEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("EndDateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("StartDateTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("TimeRecords");
                });

            modelBuilder.Entity("TimeTable.DataAccess.Contracts.Entities.PersonEntity", b =>
                {
                    b.HasOne("TimeTable.DataAccess.Contracts.Entities.CompanyEntity", "Company")
                        .WithMany("People")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TimeTable.DataAccess.Contracts.Entities.TimeRecordEntity", b =>
                {
                    b.HasOne("TimeTable.DataAccess.Contracts.Entities.PersonEntity", "Person")
                        .WithMany("TimeRecords")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}