using Api.Domain.Songs.Models;

namespace Api.Domain.Songs.Services;

public interface ISongService
{
    Task<Song?> GetSongAsync(int id);
    Task<IEnumerable<Song>> GetAllSongsAsync();
    Task<IEnumerable<Song>> GetSongsByDifficultyAsync(int difficulty);
    Task<IEnumerable<Song>> GetSongsByCategoryAsync(SongCategory category);
    Task<Song> CreateSongAsync(Song song);
    Task UpdateSongAsync(Song song);
    Task DeleteSongAsync(int id);
    Task<Song> GetRandomSongAsync(int? difficulty = null, SongCategory? category = null);
} 