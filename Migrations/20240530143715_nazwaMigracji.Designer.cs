﻿// <auto-generated />
using System;
using Euro2024REST.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Euro2024REST.Migrations
{
    [DbContext(typeof(ContextDb))]
    [Migration("20240530143715_nazwaMigracji")]
    partial class nazwaMigracji
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Euro2024REST.Models.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Score1")
                        .HasColumnType("integer");

                    b.Property<int>("Score2")
                        .HasColumnType("integer");

                    b.Property<string>("Team1Name")
                        .HasColumnType("text");

                    b.Property<string>("Team2Name")
                        .HasColumnType("text");

                    b.Property<string>("Venue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Team1Name");

                    b.HasIndex("Team2Name");

                    b.ToTable("Match");
                });

            modelBuilder.Entity("Euro2024REST.Models.Player", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<int>("MarketValue")
                        .HasColumnType("integer");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.HasIndex("TeamName");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("Euro2024REST.Models.Team", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.HasKey("Name");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("Euro2024REST.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(24)");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Euro2024REST.Models.Match", b =>
                {
                    b.HasOne("Euro2024REST.Models.Team", "Team1")
                        .WithMany()
                        .HasForeignKey("Team1Name");

                    b.HasOne("Euro2024REST.Models.Team", "Team2")
                        .WithMany()
                        .HasForeignKey("Team2Name");

                    b.Navigation("Team1");

                    b.Navigation("Team2");
                });

            modelBuilder.Entity("Euro2024REST.Models.Player", b =>
                {
                    b.HasOne("Euro2024REST.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });
#pragma warning restore 612, 618
        }
    }
}
