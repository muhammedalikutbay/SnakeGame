# SnakeGame

The classic Snake game, written in C# with WPF.

## Features

- Grid-based snake movement with arrow keys
- Countdown before each round
- Score display
- Death animation when the snake hits a wall or itself
- Game over screen with restart

## Controls

| Key | Action |
|---|---|
| Any key | Start the game |
| Arrow keys | Change direction |

## Getting started

Requirements: Windows and the .NET 9 SDK (or Visual Studio 2022 17.12+).

```bash
git clone https://github.com/muhammedalikutbay/SnakeGame.git
cd SnakeGame
dotnet run --project SnakeGame
```

Or open `SnakeGame.sln` in Visual Studio and press **F5**.

## Project structure

| File | Purpose |
|---|---|
| `Game.cs` | Game state: grid, snake, food, score, movement and collisions |
| `MainWindow.xaml(.cs)` | Rendering, input, countdown and game loop |
| `Direction.cs`, `Position.cs`, `GridValue.cs` | Small value types |
| `Images.cs` | Sprite loading from `Assets/` |

## License

[MIT](LICENSE)
