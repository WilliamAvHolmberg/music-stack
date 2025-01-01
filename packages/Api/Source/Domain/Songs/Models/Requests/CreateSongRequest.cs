using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Songs.Models.Requests;

public record CreateSongRequest(
    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
    string Title,

    [Required(ErrorMessage = "Artist is required")]
    [StringLength(100, ErrorMessage = "Artist cannot be longer than 100 characters")]
    string Artist,

    [Required(ErrorMessage = "First line is required")]
    [StringLength(200, ErrorMessage = "First line cannot be longer than 200 characters")]
    string FirstLine,

    int? Year,

    [Required(ErrorMessage = "Difficulty is required")]
    [Range(1, 3, ErrorMessage = "Difficulty must be between 1 and 3")]
    int Difficulty,

    [Required(ErrorMessage = "Category is required")]
    SongCategory Category,

    [Required(ErrorMessage = "Language is required")]
    SongLanguage Language,

    string? SpotifyId
); 