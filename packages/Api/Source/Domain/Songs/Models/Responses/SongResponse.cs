namespace Api.Domain.Songs.Models.Responses;

public record SongResponse(
    int Id,
    string Title,
    string Artist,
    string FirstLine,
    int? Year,
    int Difficulty,
    SongCategory Category,
    SongLanguage Language,
    string? SpotifyId
)
{
    public static SongResponse FromSong(Song song) => new(
        song.Id,
        song.Title,
        song.Artist,
        song.FirstLine,
        song.Year,
        song.Difficulty,
        song.Category,
        song.Language,
        song.SpotifyId
    );
} 