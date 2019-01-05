﻿// <auto-generated />
using System;
using CalendarPlanner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Calendar.Migrations
{
    [DbContext(typeof(CalendarContext))]
    partial class CalendarContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CalendarPlanner.Calendar", b =>
                {
                    b.Property<int>("CalendarId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Owner");

                    b.HasKey("CalendarId");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("CalendarPlanner.Plan", b =>
                {
                    b.Property<int>("PlanId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Activity");

                    b.Property<int?>("CalendarId");

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("PlanId");

                    b.HasIndex("CalendarId");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("CalendarPlanner.Plan", b =>
                {
                    b.HasOne("CalendarPlanner.Calendar", "Calendar")
                        .WithMany("Plans")
                        .HasForeignKey("CalendarId");
                });
#pragma warning restore 612, 618
        }
    }
}
