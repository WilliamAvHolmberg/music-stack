using Microsoft.EntityFrameworkCore;
using Api.Study;
using Api.Flashcards;

namespace Api.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<StudyStructure> StudyStructures { get; set; }
    public DbSet<Concept> Concepts { get; set; }
    public DbSet<Relationship> Relationships { get; set; }
    public DbSet<FlashcardSet> FlashcardSets { get; set; }
    public DbSet<Flashcard> Flashcards { get; set; }
    public DbSet<FlashcardReview> FlashcardReviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StudyStructure>(entity =>
        {
            entity.HasMany(e => e.Concepts)
                .WithOne(e => e.StudyStructure)
                .HasForeignKey(e => e.StudyStructureId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Relationships)
                .WithOne(e => e.StudyStructure)
                .HasForeignKey(e => e.StudyStructureId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Relationship>(entity =>
        {
            entity.HasOne(e => e.SourceConcept)
                .WithMany()
                .HasForeignKey(e => e.SourceConceptId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TargetConcept)
                .WithMany()
                .HasForeignKey(e => e.TargetConceptId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FlashcardSet>(entity =>
        {
            entity.HasMany(e => e.Flashcards)
                .WithOne(e => e.FlashcardSet)
                .HasForeignKey(e => e.FlashcardSetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.HasMany(e => e.ReviewHistory)
                .WithOne(e => e.Flashcard)
                .HasForeignKey(e => e.FlashcardId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<FlashcardReview>()
            .HasIndex(e => new { e.FlashcardId, e.ReviewedAt });
    }
} 