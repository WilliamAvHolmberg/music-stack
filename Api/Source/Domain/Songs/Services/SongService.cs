using Api.Domain.Songs.Models;
using Api.Domain.Songs.Repositories;

namespace Api.Domain.Songs.Services;

public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;
    private readonly Random _random = new();

    public SongService(ISongRepository songRepository)
    {
        _songRepository = songRepository;
    }

    public async Task<Song?> GetSongAsync(int id)
    {
        return await _songRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Song>> GetAllSongsAsync()
    {
        return await _songRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Song>> GetSongsByDifficultyAsync(int difficulty)
    {
        return await _songRepository.GetByDifficultyAsync(difficulty);
    }

    public async Task<IEnumerable<Song>> GetSongsByCategoryAsync(SongCategory category)
    {
        return await _songRepository.GetByCategoryAsync(category);
    }

    public async Task<Song> CreateSongAsync(Song song)
    {
        return await _songRepository.AddAsync(song);
    }

    public async Task UpdateSongAsync(Song song)
    {
        await _songRepository.UpdateAsync(song);
    }

    public async Task DeleteSongAsync(int id)
    {
        await _songRepository.DeleteAsync(id);
    }

    public async Task<Song> GetRandomSongAsync(int? difficulty = null, SongCategory? category = null)
    {
        var songs = await _songRepository.GetAllAsync();
        var filteredSongs = songs.AsEnumerable();

        if (difficulty.HasValue)
        {
            filteredSongs = filteredSongs.Where(s => s.Difficulty == difficulty.Value);
        }

        if (category.HasValue)
        {
            filteredSongs = filteredSongs.Where(s => s.Category == category.Value);
        }

        var songList = filteredSongs.ToList();
        if (!songList.Any())
        {
            throw new InvalidOperationException("No songs found matching the criteria");
        }

        var randomIndex = _random.Next(songList.Count);
        return songList[randomIndex];
    }
} 