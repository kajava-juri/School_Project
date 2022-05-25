﻿// <auto-generated />
using System;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KooliProjekt.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211203114300_SongSchedule2")]
    partial class SongSchedule2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("KooliProjekt.Data.Artist", b =>
                {
                    b.Property<int>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.HasKey("ArtistId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("KooliProjekt.Data.Schedule", b =>
                {
                    b.Property<int>("ScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.HasKey("ScheduleId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("KooliProjekt.Data.Song", b =>
                {
                    b.Property<int>("SongId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArtistId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ScheduleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Tempo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SongId");

                    b.HasIndex("ArtistId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("KooliProjekt.Data.SongSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ScheduleId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SongId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("SongId");

                    b.ToTable("SongSchedule");
                });

            modelBuilder.Entity("KooliProjekt.Data.Storage", b =>
                {
                    b.Property<int>("StorageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Kood")
                        .HasMaxLength(5)
                        .HasColumnType("TEXT");

                    b.Property<int>("SongId")
                        .HasColumnType("INTEGER");

                    b.HasKey("StorageID");

                    b.HasIndex("SongId")
                        .IsUnique();

                    b.ToTable("Storages");
                });

            modelBuilder.Entity("KooliProjekt.Data.Song", b =>
                {
                    b.HasOne("KooliProjekt.Data.Artist", "Artist")
                        .WithMany("Songs")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("KooliProjekt.Data.Schedule", "Schedule")
                        .WithMany()
                        .HasForeignKey("ScheduleId");

                    b.Navigation("Artist");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("KooliProjekt.Data.SongSchedule", b =>
                {
                    b.HasOne("KooliProjekt.Data.Schedule", null)
                        .WithMany("Songs")
                        .HasForeignKey("ScheduleId");

                    b.HasOne("KooliProjekt.Data.Song", "Song")
                        .WithMany()
                        .HasForeignKey("SongId");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("KooliProjekt.Data.Storage", b =>
                {
                    b.HasOne("KooliProjekt.Data.Song", "Song")
                        .WithOne("Storage")
                        .HasForeignKey("KooliProjekt.Data.Storage", "SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Song");
                });

            modelBuilder.Entity("KooliProjekt.Data.Artist", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("KooliProjekt.Data.Schedule", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("KooliProjekt.Data.Song", b =>
                {
                    b.Navigation("Storage");
                });
#pragma warning restore 612, 618
        }
    }
}
