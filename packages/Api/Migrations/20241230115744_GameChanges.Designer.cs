﻿// <auto-generated />
using System;
using Api.Shared.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241230115744_GameChanges")]
    partial class GameChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Api.Domain.Games.Models.GameSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrentItemIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentRoundIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameTemplateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameTemplateId");

                    b.ToTable("GameSessions");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.GameTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GameTemplates");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.Round", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CurrentSongId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameSessionId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GameTemplateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Instructions")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPaused")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeInMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeLeft")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CurrentSongId");

                    b.HasIndex("GameSessionId");

                    b.HasIndex("GameTemplateId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.RoundItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtraInfo")
                        .HasColumnType("TEXT");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Points")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoundId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoundId");

                    b.ToTable("RoundItems");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("GameSessionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Score")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameSessionId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Api.Domain.Songs.Models.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Difficulty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstLine")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Language")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.GameSession", b =>
                {
                    b.HasOne("Api.Domain.Games.Models.GameTemplate", "GameTemplate")
                        .WithMany("GameSessions")
                        .HasForeignKey("GameTemplateId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("GameTemplate");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.Round", b =>
                {
                    b.HasOne("Api.Domain.Songs.Models.Song", "CurrentSong")
                        .WithMany()
                        .HasForeignKey("CurrentSongId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Api.Domain.Games.Models.GameSession", "GameSession")
                        .WithMany("Rounds")
                        .HasForeignKey("GameSessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Api.Domain.Games.Models.GameTemplate", "GameTemplate")
                        .WithMany("Rounds")
                        .HasForeignKey("GameTemplateId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("CurrentSong");

                    b.Navigation("GameSession");

                    b.Navigation("GameTemplate");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.RoundItem", b =>
                {
                    b.HasOne("Api.Domain.Games.Models.Round", "Round")
                        .WithMany("Items")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Round");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.Team", b =>
                {
                    b.HasOne("Api.Domain.Games.Models.GameSession", "GameSession")
                        .WithMany("Teams")
                        .HasForeignKey("GameSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameSession");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.GameSession", b =>
                {
                    b.Navigation("Rounds");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.GameTemplate", b =>
                {
                    b.Navigation("GameSessions");

                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("Api.Domain.Games.Models.Round", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
