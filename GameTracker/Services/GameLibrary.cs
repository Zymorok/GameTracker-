using GameTracker.Models;

namespace GameTracker.Services;

public sealed class GameLibrary
{
    private readonly List<Game> _games = [];
    private int _nextId = 1;

    public IReadOnlyList<Game> GetAll()
    {
        return _games
            .OrderBy(game => game.Status)
            .ThenBy(game => game.Title)
            .ToList();
    }

    public Game Add(string title, string genre, string platform, GameStatus status, int? rating, string? notes)
    {
        var game = new Game(_nextId++, title.Trim(), genre.Trim(), platform.Trim(), status, rating, Normalize(notes));
        _games.Add(game);
        return game;
    }

    public IReadOnlyList<Game> FindByTitle(string query)
    {
        return _games
            .Where(game => game.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
            .OrderBy(game => game.Title)
            .ToList();
    }

    public IReadOnlyList<Game> FilterByStatus(GameStatus status)
    {
        return _games
            .Where(game => game.Status == status)
            .OrderBy(game => game.Title)
            .ToList();
    }

    public Game? GetById(int id)
    {
        return _games.FirstOrDefault(game => game.Id == id);
    }

    public IReadOnlyDictionary<GameStatus, int> GetStatusSummary()
    {
        return Enum.GetValues<GameStatus>()
            .ToDictionary(status => status, status => _games.Count(game => game.Status == status));
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
