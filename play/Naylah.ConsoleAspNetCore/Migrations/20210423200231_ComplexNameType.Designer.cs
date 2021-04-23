﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Naylah.ConsoleAspNetCore.ORM;

namespace Naylah.ConsoleAspNetCore.Migrations
{
    [DbContext(typeof(TestDbContext))]
    [Migration("20210423200231_ComplexNameType")]
    partial class ComplexNameType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Test")
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Naylah.ConsoleAspNetCore.Entities.Person", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("CreatedAt");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset?>("UpdatedAt");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });
#pragma warning restore 612, 618
        }
    }
}
