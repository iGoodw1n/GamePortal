using System.Collections.Concurrent;
using GamePortal.Helpers;
using GamePortal.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GamePortal.Services;

public class GameService
{
    private readonly IMemoryCache _cache;

    private readonly ConcurrentDictionary<string, DateTime> _gameIds = new();

    public event Action OnChange;

    public GameService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public IGame? GetGame<T>(string gameId) where T : IGame, new()
    {
        _gameIds.AddOrUpdate(gameId, _ = DateTime.UtcNow, (_, _) => DateTime.UtcNow);
        var game = _cache.GetOrCreate(gameId, cacheEntry =>
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(600);
            cacheEntry.AbsoluteExpiration = DateTime.UtcNow + TimeSpan.FromSeconds(3600);
            cacheEntry.Priority = CacheItemPriority.Normal;
            return new T();
        });

        Task.Run(async () =>
        {
            await Task.Delay(1000);
            OnChange?.Invoke();
        });
        return game;
    }

    public IEnumerable<GameRoom> GetGameRooms()
    {
        List<GameRoom> games = new ();
        foreach (var gameId in _gameIds.Keys)
        {
            var data = _cache.Get(gameId);
            if (data is not null)
            {
                games.Add(new GameRoom(gameId, (IGame)data));
            }
        }
        return games;
    }

    public void Update()
    {
        OnChange?.Invoke();
    }

    public IEnumerable<string> GetGameIds()
    {
        return _gameIds.Keys;
    }

    public ConcurrentDictionary<string, DateTime> GetGameStates()
    {
        return _gameIds;
    }
}
