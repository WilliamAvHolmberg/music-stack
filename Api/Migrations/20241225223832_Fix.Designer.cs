﻿// <auto-generated />
using System;
using Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241225223832_Fix")]
    partial class Fix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Api.Flashcards.Flashcard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ConceptId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("FlashcardSetId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Importance")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMarkedForReview")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastReviewDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("NextReviewDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ReviewCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SuccessCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ConceptId");

                    b.HasIndex("FlashcardSetId");

                    b.ToTable("Flashcards");
                });

            modelBuilder.Entity("Api.Flashcards.FlashcardReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<int>("FlashcardId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ReviewedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FlashcardId", "ReviewedAt");

                    b.ToTable("FlashcardReviews");
                });

            modelBuilder.Entity("Api.Flashcards.FlashcardSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<int?>("StudyStructureId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StudyStructureId");

                    b.ToTable("FlashcardSets");
                });

            modelBuilder.Entity("Api.Study.Concept", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StudyStructureId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("StudyStructureId");

                    b.ToTable("Concepts");
                });

            modelBuilder.Entity("Api.Study.Relationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SourceConceptId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StudyStructureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetConceptId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SourceConceptId");

                    b.HasIndex("StudyStructureId");

                    b.HasIndex("TargetConceptId");

                    b.ToTable("Relationships");
                });

            modelBuilder.Entity("Api.Study.StudyStructure", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalContent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("StudyStructures");
                });

            modelBuilder.Entity("Api.Flashcards.Flashcard", b =>
                {
                    b.HasOne("Api.Study.Concept", "Concept")
                        .WithMany()
                        .HasForeignKey("ConceptId");

                    b.HasOne("Api.Flashcards.FlashcardSet", "FlashcardSet")
                        .WithMany("Flashcards")
                        .HasForeignKey("FlashcardSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Concept");

                    b.Navigation("FlashcardSet");
                });

            modelBuilder.Entity("Api.Flashcards.FlashcardReview", b =>
                {
                    b.HasOne("Api.Flashcards.Flashcard", "Flashcard")
                        .WithMany("ReviewHistory")
                        .HasForeignKey("FlashcardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flashcard");
                });

            modelBuilder.Entity("Api.Flashcards.FlashcardSet", b =>
                {
                    b.HasOne("Api.Study.StudyStructure", "StudyStructure")
                        .WithMany()
                        .HasForeignKey("StudyStructureId");

                    b.Navigation("StudyStructure");
                });

            modelBuilder.Entity("Api.Study.Concept", b =>
                {
                    b.HasOne("Api.Study.StudyStructure", "StudyStructure")
                        .WithMany("Concepts")
                        .HasForeignKey("StudyStructureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StudyStructure");
                });

            modelBuilder.Entity("Api.Study.Relationship", b =>
                {
                    b.HasOne("Api.Study.Concept", "SourceConcept")
                        .WithMany()
                        .HasForeignKey("SourceConceptId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Api.Study.StudyStructure", "StudyStructure")
                        .WithMany("Relationships")
                        .HasForeignKey("StudyStructureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Study.Concept", "TargetConcept")
                        .WithMany()
                        .HasForeignKey("TargetConceptId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SourceConcept");

                    b.Navigation("StudyStructure");

                    b.Navigation("TargetConcept");
                });

            modelBuilder.Entity("Api.Flashcards.Flashcard", b =>
                {
                    b.Navigation("ReviewHistory");
                });

            modelBuilder.Entity("Api.Flashcards.FlashcardSet", b =>
                {
                    b.Navigation("Flashcards");
                });

            modelBuilder.Entity("Api.Study.StudyStructure", b =>
                {
                    b.Navigation("Concepts");

                    b.Navigation("Relationships");
                });
#pragma warning restore 612, 618
        }
    }
}
