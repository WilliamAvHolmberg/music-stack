using Api.Domain.Games.Models;
using Api.Domain.Songs.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Shared.Infrastructure.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Song> Songs => Set<Song>();
    public DbSet<GameSession> GameSessions => Set<GameSession>();
    public DbSet<GameTemplate> GameTemplates => Set<GameTemplate>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Round> Rounds => Set<Round>();
    public DbSet<RoundItem> RoundItems => Set<RoundItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Song configuration
        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Artist).IsRequired();
            entity.Property(e => e.FirstLine).IsRequired();
            entity.Property(e => e.Difficulty).IsRequired();
            entity.Property(e => e.Category).IsRequired();
            entity.Property(e => e.Language).IsRequired();
        });
        
        // GameSession configuration
        modelBuilder.Entity<GameSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Status).IsRequired();

            // Relationships
            entity.HasMany(e => e.Teams)
                .WithOne(e => e.GameSession)
                .HasForeignKey(e => e.GameSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Rounds)
                .WithOne(e => e.GameSession)
                .HasForeignKey(e => e.GameSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.GameTemplate)
                .WithMany(e => e.GameSessions)
                .HasForeignKey(e => e.GameTemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        // Team configuration
        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Color).IsRequired();
        });
        
        // Round configuration
        modelBuilder.Entity<Round>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.Title).IsRequired();
            
            // Relationships
            entity.HasOne(e => e.CurrentSong)
                .WithMany()
                .HasForeignKey(e => e.CurrentSongId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.GameTemplate)
                .WithMany(e => e.Rounds)
                .HasForeignKey(e => e.GameTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Items)
                .WithOne(e => e.Round)
                .HasForeignKey(e => e.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RoundItem configuration
        modelBuilder.Entity<RoundItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Artist).IsRequired();
            entity.Property(e => e.OrderIndex).IsRequired();
        });

        // GameTemplate configuration
        modelBuilder.Entity<GameTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
} 