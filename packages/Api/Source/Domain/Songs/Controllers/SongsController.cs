using Api.Domain.Songs.Models;
using Api.Domain.Songs.Models.Requests;
using Api.Domain.Songs.Models.Responses;
using Api.Domain.Songs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Domain.Songs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongsController : ControllerBase
{
    private readonly ISongService _songService;
    private readonly ILogger<SongsController> _logger;

    public SongsController(ISongService songService, ILogger<SongsController> logger)
    {
        _songService = songService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SongResponse>>> GetSongs()
    {
        var songs = await _songService.GetAllSongsAsync();
        return Ok(songs.Select(SongResponse.FromSong));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SongResponse>> GetSong(int id)
    {
        var song = await _songService.GetSongAsync(id);
        if (song == null)
        {
            return NotFound();
        }
        return Ok(SongResponse.FromSong(song));
    }

    [HttpGet("difficulty/{difficulty}")]
    public async Task<ActionResult<IEnumerable<SongResponse>>> GetSongsByDifficulty(int difficulty)
    {
        if (difficulty < 1 || difficulty > 3)
        {
            return BadRequest("Difficulty must be between 1 and 3");
        }
        var songs = await _songService.GetSongsByDifficultyAsync(difficulty);
        return Ok(songs.Select(SongResponse.FromSong));
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<SongResponse>>> GetSongsByCategory(SongCategory category)
    {
        var songs = await _songService.GetSongsByCategoryAsync(category);
        return Ok(songs.Select(SongResponse.FromSong));
    }

    [HttpPost]
    public async Task<ActionResult<SongResponse>> CreateSong(CreateSongRequest request)
    {
        try
        {
            var song = new Song
            {
                Title = request.Title,
                Artist = request.Artist,
                FirstLine = request.FirstLine,
                Year = request.Year,
                Difficulty = request.Difficulty,
                Category = request.Category,
                Language = request.Language,
                SpotifyId = request.SpotifyId
            };

            var createdSong = await _songService.CreateSongAsync(song);
            return CreatedAtAction(
                nameof(GetSong),
                new { id = createdSong.Id },
                SongResponse.FromSong(createdSong));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating song");
            return StatusCode(500, "An error occurred while creating the song");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSong(int id, CreateSongRequest request)
    {
        try
        {
            var existingSong = await _songService.GetSongAsync(id);
            if (existingSong == null)
            {
                return NotFound();
            }

            existingSong.Title = request.Title;
            existingSong.Artist = request.Artist;
            existingSong.FirstLine = request.FirstLine;
            existingSong.Year = request.Year;
            existingSong.Difficulty = request.Difficulty;
            existingSong.Category = request.Category;
            existingSong.Language = request.Language;
            existingSong.SpotifyId = request.SpotifyId;

            await _songService.UpdateSongAsync(existingSong);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating song");
            return StatusCode(500, "An error occurred while updating the song");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSong(int id)
    {
        try
        {
            await _songService.DeleteSongAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting song");
            return StatusCode(500, "An error occurred while deleting the song");
        }
    }
} 