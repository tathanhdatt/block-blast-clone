using System.Collections.Generic;

public static class BoardCellUtility
{
    public static bool IsFull(this BoardCell[,] boardCells, int? row = null, int? column = null)
    {
        int numberOfOccupiedCells = 0;
        foreach (BoardCell boardCell in boardCells)
        {
            if (!boardCell.IsOccupied) continue;
            if (column != null && boardCell.X == column) numberOfOccupiedCells++;
            else if (row != null && boardCell.Y == row) numberOfOccupiedCells++;
        }

        return numberOfOccupiedCells == GameConstant.boardSize;
    }

    public static bool IsFullIfPlaced(this BoardCell[,] boardCells,
        int? row = null, int? column = null)
    {
        int numberOfOccupiedCells = 0;
        foreach (BoardCell boardCell in boardCells)
        {
            if (!boardCell.IsOccupied && !boardCell.IsHovering) continue;
            if (column != null && boardCell.X == column) numberOfOccupiedCells++;
            else if (row != null && boardCell.Y == row) numberOfOccupiedCells++;
        }

        return numberOfOccupiedCells == GameConstant.boardSize;
    }

    public static IList<int> GetCompletedRowsIfPlaced(this BoardCell[,] board, IList<int> rows)
    {
        IList<int> completedRows = new List<int>();
        foreach (int row in rows)
        {
            if (board.IsFullIfPlaced(row: row))
            {
                completedRows.Add(row);
            }
        }

        return completedRows;
    }

    public static IList<int> GetCompletedRows(this BoardCell[,] board, IList<int> rows)
    {
        IList<int> completedRows = new List<int>();
        foreach (int row in rows)
        {
            if (board.IsFull(row: row))
            {
                completedRows.Add(row);
            }
        }

        return completedRows;
    }

    public static IList<int> GetCompletedColumnsIfPlaced(this BoardCell[,] board, IList<int> columns)
    {
        IList<int> completedColumns = new List<int>();
        foreach (int column in columns)
        {
            if (board.IsFullIfPlaced(column: column))
            {
                completedColumns.Add(column);
            }
        }

        return completedColumns;
    }

    public static IList<int> GetCompletedColumns(this BoardCell[,] board, IList<int> columns)
    {
        IList<int> completedColumns = new List<int>();
        foreach (int column in columns)
        {
            if (board.IsFull(column: column))
            {
                completedColumns.Add(column);
            }
        }

        return completedColumns;
    }

    public static void ClearRows(this BoardCell[,] board, IList<int> rows)
    {
        foreach (int row in rows)
        {
            board.Clear(row: row);
        }
    }
    
    public static void ClearColumns(this BoardCell[,] board, IList<int> cols)
    {
        foreach (int col in cols)
        {
            board.Clear(col: col);
        }
    }

    public static void Clear(this BoardCell[,] board, int? col = null, int? row = null)
    {
        foreach (BoardCell boardCell in board)
        {
            if (col != null && boardCell.X == col) boardCell.Clear();
            else if (row != null && boardCell.Y == row) boardCell.Clear();
        }
    }
}