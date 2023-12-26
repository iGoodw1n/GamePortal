
using GamePortal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace GamePortal.Workers;

public class GameCleanerService : BackgroundService
{
    private readonly ConcurrentDictionary<string, DateTime> _gameStates;

    public GameCleanerService([FromServices] GameService gameService)
    {
        _gameStates = gameService.GetGameStates();
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1800));

        while (!stoppingToken.IsCancellationRequested &&
            await timer.WaitForNextTickAsync(stoppingToken))
        {
            foreach (var gameId in _gameStates.Keys)
            {
                if (DateTime.UtcNow - _gameStates[gameId] > TimeSpan.FromSeconds(1800))
                {
                    _gameStates.Remove(gameId, out var _);
                }
            }
        }
    }
}
