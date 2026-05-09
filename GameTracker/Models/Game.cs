namespace GameTracker.Models;

public sealed class Game
{
    public Game(int id, string title, string genre, string platform, GameStatus status, int? rating = null, string? notes = null)
    {
        Id = id;
        Title = title;
        Genre = genre;
        Platform = platform;
        Status = status;
        Rating = rating;
        Notes = notes;
        CreatedAt = DateTimeOffset.Now;
    }

    public int Id { get; }
    public string Title { get; }
    public string Genre { get; }
    public string Platform { get; }
    public GameStatus Status { get; private set; }
    public int? Rating { get; private set; }
    public string? Notes { get; private set; }
    public DateTimeOffset CreatedAt { get; }

    public void UpdateProgress(GameStatus status, int? rating, string? notes)
    {
        Status = status;
        Rating = rating;
        Notes = string.IsNullOrWhiteSpace(notes) ? Notes : notes.Trim();
    }
}
