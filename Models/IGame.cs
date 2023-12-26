namespace GamePortal.Models;

public interface IGame
{
    string Winner { get; set; }

    string Player1 { get; set; }

    string Player2 { get; set; }

    string Player1Name { get; set; }

    string Player2Name { get; set; }

    List<(int, int)> WinCoords { get; set; }

    ResultOfGame Result { get; set; }

    event Action OnChange;

    char?[,] GetField();

    bool IsDraw();

    bool IsStarted();

    bool IsFinished();

    void AddPlayer(string playerId, string playerName);

    void HandleNextMove((int x, int y) position, string player);

    string? GetNextPlayer();

    void EndGame(string winnerId);

    void Reset();

    //void SetUserPresence(string userId);

    void NotifyChange();
    //void CheckPresence();
}

public enum ResultOfGame
{
    Continue,
    Winner,
    Draw
}
