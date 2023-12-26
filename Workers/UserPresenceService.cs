using GamePortal.Models;
using GamePortal.Services;
using Microsoft.Extensions.Caching.Memory;

namespace GamePortal.Workers;

public class UserPresenceService : BackgroundService
{
    private readonly IMemoryCache _cache;
    private readonly GameService _gameService;

    public UserPresenceService(IMemoryCache cache, GameService gameService)
    {
        _cache = cache;
        _gameService = gameService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while (!stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            HandleGames();
        }
    }

    private void HandleGames()
    {
        foreach (var gameId in _gameService.GetGameIds())
        {
            EndGameIfUserOffline(gameId);
        }
    }

    private void EndGameIfUserOffline(string gameId)
    {
        var data = _cache.Get(gameId);
        if (data is not null)
        {
            HandleUserPresence(data);
        }
    }

    private void HandleUserPresence(object data)
    {
        var game = (IGame)data;
        if (game.IsStarted() && !game.IsFinished())
        {
            var player1LastChecked = _cache.Get(game.Player1);
            var player2LastChecked = _cache.Get(game.Player2);
            if (player1LastChecked is null || player2LastChecked is null) return;
            CheckPlayer(game, (DateTime)player1LastChecked, game.Player2);
            CheckPlayer(game, (DateTime)player2LastChecked, game.Player1);
        }
    }

    private void CheckPlayer(IGame game, DateTime playerLastChecked, string winner)
    {
        if (DateTime.UtcNow - playerLastChecked > TimeSpan.FromSeconds(5))
        {
            game.EndGame(winner);
            _gameService.Update();
        }
    }
}
