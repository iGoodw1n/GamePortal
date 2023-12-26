using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace GamePortal.Hubs;

public class GameHub : Hub
{
    private readonly IMemoryCache cache;

    public GameHub(IMemoryCache cache)
    {
        this.cache = cache;
    }

    public async override Task OnConnectedAsync()
    {
        var group = Context.GetHttpContext()!.Request.Query["gameId"];

        string value = !string.IsNullOrEmpty(group.ToString()) ? group.ToString() : "default";

        Context.Items["groupId"] = group;

        await Groups.AddToGroupAsync(Context.ConnectionId, value);
        await base.OnConnectedAsync();
    }

    public async Task Ping(string userId)
    {
        //await Clients.OthersInGroup(Context.Items["groupId"]!.ToString()!).SendAsync("Ping");

        //await Clients.Caller.SendAsync("Ping");

        cache.Set(userId, DateTime.UtcNow);
    }
}
