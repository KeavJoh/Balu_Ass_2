﻿// <auto-generated />
using System;
using Balu_Ass_2.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Balu_Ass_2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240207164612_addChildrenGroupInChildDeregistration")]
    partial class addChildrenGroupInChildDeregistration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Balu_Ass_2.Modals.ChildDeregistration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChildId")
                        .HasColumnType("int");

                    b.Property<int>("ChildrenGroup")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateDeregistrationOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeregistrationBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeregistrationDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChildDeregistrations");
                });

            modelBuilder.Entity("Balu_Ass_2.Modals.ChildExpiredDeregistration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChildId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateDeregistrationOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeregistrationBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeregistrationDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChildExpiredDeregistrations");
                });

            modelBuilder.Entity("Balu_Ass_2.Modals.ChildWithdrawnDeregistration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChildId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateDeregistrationOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateWithdrawnDeregistration")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeregistrationBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DeregistrationDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WithdrawnBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChildWithdrawnDeregistrations");
                });

            modelBuilder.Entity("Balu_Ass_2.Modals.Children", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChildrenGroup")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfLogged")
                        .HasColumnType("datetime2");

                    b.Property<string>("Father")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mother")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Childrens");
                });

            modelBuilder.Entity("Balu_Ass_2.Modals.DeletedChildren", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfDeletion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfLogged")
                        .HasColumnType("datetime2");

                    b.Property<string>("Father")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mother")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DeletedChildrens");
                });

            modelBuilder.Entity("Balu_Ass_2.Modals.SystemLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeOfAction")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SystemLogs");
                });
#pragma warning restore 612, 618
        }
    }
}