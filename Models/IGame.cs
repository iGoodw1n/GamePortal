namespace GamePortal.Models;

public interface IGame
{
    string Winner { get; set; }
    List<(int, int)> WinCoords { get; set; }
    ResultOfGame Result { get; set; }

    event Action OnChange;
    char?[,] GetField();

    bool IsDraw();

    bool IsStarted();

    void AddPlayer(string playerId, string playerName);

    void HandleNextMove((int x, int y) position, string player);

    string? GetNextPlayer();

    void Reset();

    void NotifyChange();
}

public enum ResultOfGame
{
    Continue,
    Winner,
    Draw
}
