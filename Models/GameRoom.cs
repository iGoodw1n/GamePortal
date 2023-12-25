namespace GamePortal.Models;

public class GameRoom
{
    public string Id { get; set; }
    public User Player1 { get; set; }

    public User Player2 { get; set; }

    public IGame Game { get; set; }
}
