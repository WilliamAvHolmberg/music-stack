using Api.Domain.Songs.Models;
using Api.Shared.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Songs.Services;

public class SongService : ISongService
{
    private readonly AppDbContext _context;
    private readonly Random _random = new();

    public SongService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Song?> GetSongAsync(int id)
    {
        return await _context.Songs.FindAsync(id);
    }

    public async Task<IEnumerable<Song>> GetAllSongsAsync()
    {
        return await _context.Songs.ToListAsync();
    }

    public async Task<IEnumerable<Song>> GetSongsByDifficultyAsync(int difficulty)
    {
        return await _context.Songs
            .Where(s => s.Difficulty == difficulty)
            .ToListAsync();
    }

    public async Task<IEnumerable<Song>> GetSongsByCategoryAsync(SongCategory category)
    {
        return await _context.Songs
            .Where(s => s.Category == category)
            .ToListAsync();
    }

    public async Task<Song> CreateSongAsync(Song song)
    {
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();
        return song;
    }

    public async Task UpdateSongAsync(Song song)
    {
        _context.Entry(song).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSongAsync(int id)
    {
        var song = await _context.Songs.FindAsync(id);
        if (song != null)
        {
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Song> GetRandomSongAsync(int? difficulty = null, SongCategory? category = null)
    {
        var query = _context.Songs.AsQueryable();

        if (difficulty.HasValue)
        {
            query = query.Where(s => s.Difficulty == difficulty.Value);
        }

        if (category.HasValue)
        {
            query = query.Where(s => s.Category == category.Value);
        }

        var count = await query.CountAsync();
        if (count == 0)
        {
            throw new InvalidOperationException("No songs found matching the criteria");
        }

        var randomIndex = _random.Next(count);
        return await query.Skip(randomIndex).FirstAsync();
    }
} 