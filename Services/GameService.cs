using System.Collections.Concurrent;
using GamePortal.Models;

namespace GamePortal.Services;

public static class GameService
{
    private static readonly ConcurrentDictionary<string, IGame> _games = new();

    public static IGame GetGame<T>(string gameId) where T : IGame, new()
    {
        return _games.GetOrAdd(gameId, new T());
    }

    public static void SetGame<T>(string gameId, T game) where T : IGame, new()
    {
        _games.AddOrUpdate(gameId, game, (_, _) => game);
    }
}
