﻿// <auto-generated />
using CommandsService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CommandsService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CommandsService.Models.Commands", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CommandLine")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HowTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlatformId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlatformId");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("CommandsService.Models.Platform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ExternalID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Platforms");
                });

            modelBuilder.Entity("CommandsService.Models.Commands", b =>
                {
                    b.HasOne("CommandsService.Models.Platform", "Platform")
                        .WithMany("Commands")
                        .HasForeignKey("PlatformId");

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("CommandsService.Models.Platform", b =>
                {
                    b.Navigation("Commands");
                });
#pragma warning restore 612, 618
        }
    }
}
