# GameTracker

GameTracker is a simple C# console application for keeping track of games and their play status.

## Features

- Add games with title, genre, platform, status, rating, and notes.
- View the full game list.
- Filter games by status.
- Search games by title.
- Update a game's status and rating.
- Show a small library summary.

## Requirements

- .NET 8 SDK or newer

## Run

```bash
dotnet run --project GameTracker/GameTracker.csproj
```

## Build

```bash
dotnet build GameTracker.sln
```

## Docker

Build the image locally:

```bash
docker build -t zymorok/gametracker .
```

Run the container:

```bash
docker run --rm -it zymorok/gametracker
```

GitHub Actions publishes the image to Docker Hub on every push to `main`.
The repository must contain these Actions secrets:

- `DOCKER_USERNAME`
- `DOCKER_PASSWORD`
