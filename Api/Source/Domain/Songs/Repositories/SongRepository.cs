using Api.Domain.Songs.Models;
using Api.Shared.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Domain.Songs.Repositories;

public class SongRepository : ISongRepository
{
    private readonly AppDbContext _context;

    public SongRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Song?> GetByIdAsync(int id)
    {
        return await _context.Songs.FindAsync(id);
    }

    public async Task<IEnumerable<Song>> GetAllAsync()
    {
        return await _context.Songs.ToListAsync();
    }

    public async Task<IEnumerable<Song>> GetByDifficultyAsync(int difficulty)
    {
        return await _context.Songs
            .Where(s => s.Difficulty == difficulty)
            .ToListAsync();
    }

    public async Task<IEnumerable<Song>> GetByCategoryAsync(SongCategory category)
    {
        return await _context.Songs
            .Where(s => s.Category == category)
            .ToListAsync();
    }

    public async Task<Song> AddAsync(Song song)
    {
        _context.Songs.Add(song);
        await _context.SaveChangesAsync();
        return song;
    }

    public async Task UpdateAsync(Song song)
    {
        _context.Entry(song).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var song = await GetByIdAsync(id);
        if (song != null)
        {
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();
        }
    }
} 