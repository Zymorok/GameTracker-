using GameTracker.Models;

namespace GameTracker.Services;

public sealed class ConsoleGameApp
{
    private readonly GameLibrary _library;

    public ConsoleGameApp(GameLibrary library)
    {
        _library = library;
    }

    public void Run()
    {
        SeedDemoData();

        while (true)
        {
            PrintMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddGame();
                    break;
                case "2":
                    PrintGames(_library.GetAll());
                    break;
                case "3":
                    SearchByTitle();
                    break;
                case "4":
                    FilterByStatus();
                    break;
                case "5":
                    UpdateGame();
                    break;
                case "6":
                    PrintSummary();
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Unknown option. Try again.");
                    break;
            }
        }
    }

    private void SeedDemoData()
    {
        _library.Add("The Witcher 3", "RPG", "PC", GameStatus.Completed, 10, "Finished main story.");
        _library.Add("Hades", "Roguelike", "PC", GameStatus.Playing, 9, "Great combat loop.");
        _library.Add("Celeste", "Platformer", "Nintendo Switch", GameStatus.Planned, null, null);
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== GameTracker ===");
        Console.WriteLine("1. Add game");
        Console.WriteLine("2. Show all games");
        Console.WriteLine("3. Search by title");
        Console.WriteLine("4. Filter by status");
        Console.WriteLine("5. Update game progress");
        Console.WriteLine("6. Show summary");
        Console.WriteLine("0. Exit");
        Console.Write("Choose option: ");
    }

    private void AddGame()
    {
        var title = ReadRequired("Title: ");
        var genre = ReadRequired("Genre: ");
        var platform = ReadRequired("Platform: ");
        var status = ReadStatus();
        var rating = ReadRating();

        Console.Write("Notes (optional): ");
        var notes = Console.ReadLine();

        var game = _library.Add(title, genre, platform, status, rating, notes);
        Console.WriteLine($"Added #{game.Id}: {game.Title}");
    }

    private void SearchByTitle()
    {
        var query = ReadRequired("Search title: ");
        PrintGames(_library.FindByTitle(query));
    }

    private void FilterByStatus()
    {
        var status = ReadStatus();
        PrintGames(_library.FilterByStatus(status));
    }

    private void UpdateGame()
    {
        var id = ReadInt("Game id: ");
        var game = _library.GetById(id);

        if (game is null)
        {
            Console.WriteLine("Game was not found.");
            return;
        }

        var status = ReadStatus();
        var rating = ReadRating();

        Console.Write("Notes (leave empty to keep current): ");
        var notes = Console.ReadLine();

        game.UpdateProgress(status, rating, notes);
        Console.WriteLine($"Updated #{game.Id}: {game.Title}");
    }

    private void PrintSummary()
    {
        Console.WriteLine();
        Console.WriteLine("Library summary:");

        foreach (var item in _library.GetStatusSummary())
        {
            Console.WriteLine($"- {item.Key}: {item.Value}");
        }
    }

    private static void PrintGames(IReadOnlyList<Game> games)
    {
        Console.WriteLine();

        if (games.Count == 0)
        {
            Console.WriteLine("No games found.");
            return;
        }

        foreach (var game in games)
        {
            var rating = game.Rating.HasValue ? $"{game.Rating}/10" : "not rated";
            var notes = string.IsNullOrWhiteSpace(game.Notes) ? string.Empty : $" | {game.Notes}";
            Console.WriteLine($"#{game.Id} {game.Title} [{game.Platform}] - {game.Genre} - {game.Status} - {rating}{notes}");
        }
    }

    private static string ReadRequired(string label)
    {
        while (true)
        {
            Console.Write(label);
            var value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("Value cannot be empty.");
        }
    }

    private static int ReadInt(string label)
    {
        while (true)
        {
            Console.Write(label);

            if (int.TryParse(Console.ReadLine(), out var value) && value > 0)
            {
                return value;
            }

            Console.WriteLine("Enter a positive number.");
        }
    }

    private static int? ReadRating()
    {
        while (true)
        {
            Console.Write("Rating 1-10 (optional): ");
            var value = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (int.TryParse(value, out var rating) && rating is >= 1 and <= 10)
            {
                return rating;
            }

            Console.WriteLine("Rating must be between 1 and 10.");
        }
    }

    private static GameStatus ReadStatus()
    {
        while (true)
        {
            Console.WriteLine("Status:");

            foreach (var status in Enum.GetValues<GameStatus>())
            {
                Console.WriteLine($"{(int)status}. {status}");
            }

            Console.Write("Choose status: ");

            if (int.TryParse(Console.ReadLine(), out var selected) && Enum.IsDefined(typeof(GameStatus), selected))
            {
                return (GameStatus)selected;
            }

            Console.WriteLine("Unknown status.");
        }
    }
}
