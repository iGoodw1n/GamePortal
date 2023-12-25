using GamePortal.Helpers;
using System.ComponentModel;

namespace GamePortal.Models;

public class TicTacToe : IGame
{
    private char?[,] _field = new char?[3, 3];
    private string? player1;
    private string? player2;

    private string? player1Name;
    private string? player2Name;

    private string? nextMoveTurn;

    public event Action? OnChange;

    public string Winner { get; set; } = string.Empty;
    public List<(int, int)> WinCoords { get; set; } = new();
    public ResultOfGame Result { get; set; } = ResultOfGame.Continue;

    public void AddPlayer(string playerId, string playerName)
    {
        if (IsStarted()) return;
        if (player1 == null)
        {
            player1 = playerId;
            player1Name = playerName;
        }
        else
        {
            player2 = playerId;
            player2Name = playerName;
            nextMoveTurn = player1;
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
            winner = player1Name!;
            return true;
        }

        if (TryGetCoordsForVictory(DataForGames.PLAYER2_SYMBOL, out coords))
        {
            winner = player2Name!;
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
        nextMoveTurn = nextMoveTurn == player1 ? player2 : player1;
    }

    private void SetMark((int x, int y) position, char mark)
    {
        var (x, y) = position;
        _field[x, y] = mark;
    }

    private char GetMark()
    {
        return string.Equals(nextMoveTurn, player1) ? DataForGames.PLAYER1_SYMBOL : DataForGames.PLAYER2_SYMBOL;
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
        return player1 is not null && player2 is not null;
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
        nextMoveTurn = player1;
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
}
