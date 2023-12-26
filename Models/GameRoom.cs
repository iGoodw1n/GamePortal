namespace GamePortal.Models;

public class GameRoom
{
    public string Id { get; }

    public IGame Game { get; }

    public GameRoom(string id, IGame game)
    {
        Id = id;
        Game = game;
    }
}
