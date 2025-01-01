using Api.Domain.Songs.Models;

namespace Api.Domain.Songs.Repositories;

public interface ISongRepository
{
    Task<Song?> GetByIdAsync(int id);
    Task<IEnumerable<Song>> GetAllAsync();
    Task<IEnumerable<Song>> GetByDifficultyAsync(int difficulty);
    Task<IEnumerable<Song>> GetByCategoryAsync(SongCategory category);
    Task<Song> AddAsync(Song song);
    Task UpdateAsync(Song song);
    Task DeleteAsync(int id);
} 