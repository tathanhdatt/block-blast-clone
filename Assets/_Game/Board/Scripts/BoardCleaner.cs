using UnityEngine;

public class BoardCleaner
{
    private BoardCell[,] board;

    public BoardCleaner(BoardCell[,] board)
    {
        this.board = board;
    }

    public void CleanRowIfFull(int id)
    {
        if (IsFull(y: id))
        {
            Clear(y: id);
        }
    }

    public void CleanColumnIfFull(int id)
    {
        if (IsFull(x: id))
        {
            Clear(x: id);
        }
    }

    private bool IsFull(int? x = null, int? y = null)
    {
        int numberOfOccupiedCells = 0;
        foreach (BoardCell boardCell in this.board)
        {
            if (!boardCell.IsOccupied) continue;
            if (x != null && boardCell.X == x) numberOfOccupiedCells++;
            else if (y != null && boardCell.Y == y) numberOfOccupiedCells++;
        }

        return numberOfOccupiedCells == BoardConstant.boardSize;
    }

    private void Clear(int? x = null, int? y = null)
    {
        foreach (BoardCell boardCell in this.board)
        {
            if (x != null && boardCell.X == x) boardCell.Clear();
            else if (y != null && boardCell.Y == y) boardCell.Clear();
        }
    }
}