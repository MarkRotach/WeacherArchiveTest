﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherArchive.Database;

#nullable disable

namespace WeatherArchive.Database.Migrations
{
    [DbContext(typeof(WeatherArchiveDbContext))]
    partial class WeatherArchiveDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WeatherArchive.Database.Domain.ReportWindDirection", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int?>("ReportId")
                        .HasColumnType("int");

                    b.Property<int?>("WindDirectionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.HasIndex("WindDirectionId");

                    b.ToTable("ReportWindDirections");
                });

            modelBuilder.Entity("WeatherArchive.Database.Domain.WeatherReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Cloudiness")
                        .HasColumnType("int");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<double>("DewPoint")
                        .HasColumnType("float");

                    b.Property<string>("HorizontalVisibility")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Humidity")
                        .HasColumnType("float");

                    b.Property<string>("LowerCloudCover")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phenomena")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Pressure")
                        .HasColumnType("int");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("time");

                    b.Property<int?>("WindSpeed")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Date", "Time");

                    b.ToTable("WeatherReports");
                });

            modelBuilder.Entity("WeatherArchive.Database.Domain.WindDirection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WindDirections");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "С"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Ю"
                        },
                        new
                        {
                            Id = 3,
                            Name = "З"
                        },
                        new
                        {
                            Id = 4,
                            Name = "В"
                        },
                        new
                        {
                            Id = 5,
                            Name = "СЗ"
                        },
                        new
                        {
                            Id = 6,
                            Name = "СВ"
                        },
                        new
                        {
                            Id = 7,
                            Name = "ЮЗ"
                        },
                        new
                        {
                            Id = 8,
                            Name = "ЮВ"
                        });
                });

            modelBuilder.Entity("WeatherArchive.Database.Domain.ReportWindDirection", b =>
                {
                    b.HasOne("WeatherArchive.Database.Domain.WeatherReport", "Report")
                        .WithMany("WindDirections")
                        .HasForeignKey("ReportId");

                    b.HasOne("WeatherArchive.Database.Domain.WindDirection", "WindDirection")
                        .WithMany()
                        .HasForeignKey("WindDirectionId");

                    b.Navigation("Report");

                    b.Navigation("WindDirection");
                });

            modelBuilder.Entity("WeatherArchive.Database.Domain.WeatherReport", b =>
                {
                    b.Navigation("WindDirections");
                });
#pragma warning restore 612, 618
        }
    }
}
