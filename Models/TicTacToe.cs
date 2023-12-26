using GamePortal.Helpers;

namespace GamePortal.Models;

public class TicTacToe : IGame
{
    private readonly char?[,] _field = new char?[3, 3];
    public string Player1 { get; set; } = null!;
    public string Player2 { get; set; } = null!;

    public string Player1Name { get; set; } = null!;
    public string Player2Name { get; set; } = null!;

    private string? nextMoveTurn;

    public event Action? OnChange;

    bool _isGameFinished;

    public string Winner { get; set; } = string.Empty;
    public List<(int, int)> WinCoords { get; set; } = new();
    public ResultOfGame Result { get; set; } = ResultOfGame.Continue;

    public void AddPlayer(string playerId, string playerName)
    {
        if (IsStarted()) return;
        if (Player1 == null)
        {
            Player1 = playerId;
            Player1Name = playerName;
        }
        else
        {
            Player2 = playerId;
            Player2Name = playerName;
            nextMoveTurn = Player1;
            NotifyChange();
        }
    }

    public char?[,] GetField()
    {
        return _field;
    }

    public void HandleNextMove((int x, int y) position, string playerId)
    {
        if (nextMoveTurn != playerId || IsAreadyMarked(position)) return;
        HandleNextMove(position);
    }

    private bool IsAreadyMarked((int x, int y) position)
    {
        return _field[position.x, position.y] is not null;
    }

    private void HandleNextMove((int x, int y) position)
    {
        MakeMove(position);
        SetNextPlayer();
        SetResult();
    }

    private void SetResult()
    {
        if (FindWinner(out var coords, out var winner))
        {
            Winner = winner;
            WinCoords = coords;
            Result = ResultOfGame.Winner;
            return;
        }

        if (IsDraw())
        {
            Result = ResultOfGame.Draw;
            return;
        }
    }

    private bool FindWinner(out List<(int, int)> coords, out string winner)
    {
        coords = new();
        winner = string.Empty;

        if (TryGetCoordsForVictory(DataForGames.PLAYER1_SYMBOL, out coords))
        {
            winner = Player1Name!;
            return true;
        }

        if (TryGetCoordsForVictory(DataForGames.PLAYER2_SYMBOL, out coords))
        {
            winner = Player2Name!;
            return true;
        }

        return false;
    }

    private bool TryGetCoordsForVictory(char c, out List<(int, int)> coords)
    {
        if (TryGetVictoryRow(c, out coords))
        {
            return true;
        }

        if (TryGetVictoryColumn(c, out coords))
        {
            return true;
        }

        if (TryGetDiagonalVictory(c, out coords))
        {
            return true;
        }

        return false;
    }

    private bool TryGetVictoryRow(char c, out List<(int, int)> coords)
    {
        coords = new();
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int j = 0; j < _field.GetLength(1); j++)
            {
                if (_field[i, j] == c)
                {
                    coords.Add((i, j));
                }
            }
            if (coords.Count == 3)
            {
                return true;
            }
            coords.Clear();
        }

        return false;
    }

    private bool TryGetVictoryColumn(char c, out List<(int, int)> coords)
    {
        coords = new();
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int j = 0; j < _field.GetLength(1); j++)
            {
                if (_field[j, i] == c)
                {
                    coords.Add((j, i));
                }
            }
            if (coords.Count == 3)
            {
                return true;
            }
            coords.Clear();
        }

        return false;
    }

    private bool TryGetDiagonalVictory(char c, out List<(int, int)> coords)
    {
        coords = new();
        var coords1 = new List<(int, int)>();
        var coords2 = new List<(int, int)>();
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int j = 0; j < _field.GetLength(1); j++)
            {
                if (i == j && _field[i, j] == c)
                {
                    coords1.Add((i, j));
                }

                if (i == _field.GetLength(1) - j - 1 && _field[i, j] == c)
                {
                    coords2.Add((i, j));
                }
            }
        }

        if (coords1.Count == 3)
        {
            coords = coords1;
            return true;
        }

        if (coords2.Count == 3)
        {
            coords = coords2;
            return true;
        }

        return false;
    }

    private void MakeMove((int x, int y) position)
    {
        var mark = GetMark();
        SetMark(position, mark);
    }

    private void SetNextPlayer()
    {
        nextMoveTurn = nextMoveTurn == Player1 ? Player2 : Player1;
    }

    private void SetMark((int x, int y) position, char mark)
    {
        var (x, y) = position;
        _field[x, y] = mark;
    }

    private char GetMark()
    {
        return string.Equals(nextMoveTurn, Player1) ? DataForGames.PLAYER1_SYMBOL : DataForGames.PLAYER2_SYMBOL;
    }

    public bool IsDraw()
    {
        foreach (var item in _field)
        {
            if (item == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsStarted()
    {
        return Player1 is not null && Player2 is not null;
    }

    public void NotifyChange()
    {
        OnChange?.Invoke();
    }

    public string? GetNextPlayer()
    {
        return nextMoveTurn;
    }

    public void Reset()
    {
        ClearField();
        nextMoveTurn = Player1;
        Result = ResultOfGame.Continue;
    }

    private void ClearField()
    {
        for (int i = 0; i < _field.GetLength(0); i++)
        {
            for (int j = 0;  j < _field.GetLength(1); j++)
            {
                _field[i, j] = null;
            }
        }
    }

    public void EndGame(string winnerId)
    {
        Winner = winnerId == Player1 ? Player1Name : Player2Name;
        Result = ResultOfGame.Winner;
        _isGameFinished = true;
        NotifyChange();
    }

    public bool IsFinished()
    {
        return _isGameFinished;
    }

    //public void SetUserPresence(string userId)
    //{
    //    if (userId == player1)
    //    {
    //        _lastCheckedPlayer1 = DateTime.UtcNow;
    //    }
    //    else if (userId == player2)
    //    {
    //        _lastCheckedPlayer2 = DateTime.UtcNow;
    //    }
    //}

    //private void EndGameIfOffline(string winner)
    //{
    //    if (_cache.TryGetValue(winner, out var lastChecked))
    //    {
    //        if (IsStarted() && !IsFinished() && (DateTime.UtcNow - (DateTime)lastChecked! > TimeSpan.FromSeconds(5)))
    //        {
    //            EndGame(winner);
    //        }
    //    }
    //    else
    //    {
    //        EndGame(winner);
    //    }
        
    //}

    //public void CheckPresence()
    //{
    //    EndGameIfOffline(Player2!);
    //    EndGameIfOffline(Player1!);
    //}
}
