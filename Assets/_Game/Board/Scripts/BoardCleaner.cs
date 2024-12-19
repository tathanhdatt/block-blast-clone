using System.Collections.Generic;

public class BoardCleaner
{
    private readonly BoardCell[,] board;

    public BoardCleaner(BoardCell[,] board)
    {
        this.board = board;
    }

    public void Clean(List<int> rows = null, List<int> cols = null)
    {
        rows ??= CreateIntList(BoardConstant.boardSize);
        cols ??= CreateIntList(BoardConstant.boardSize);
        List<int> completedRows = GetCompletedRows(rows);
        List<int> completedColumns = GetCompletedColumns(cols);
        ClearRows(completedRows);
        ClearColumns(completedColumns);
    }

    private void ClearRows(List<int> rows)
    {
        foreach (int row in rows)
        {
            Clear(row: row);
        }
    }

    private void ClearColumns(List<int> cols)
    {
        foreach (int col in cols)
        {
            Clear(col: col);
        }
    }

    private List<int> GetCompletedRows(List<int> rows)
    {
        List<int> completedRows = new List<int>();
        foreach (int row in rows)
        {
            if (IsFull(row: row))
            {
                completedRows.Add(row);
            }
        }

        return completedRows;
    }

    private List<int> GetCompletedColumns(List<int> columns)
    {
        List<int> completedColumns = new List<int>();
        foreach (int column in columns)
        {
            if (IsFull(col: column))
            {
                completedColumns.Add(column);
            }
        }

        return completedColumns;
    }

    private bool IsFull(int? col = null, int? row = null)
    {
        int numberOfOccupiedCells = 0;
        foreach (BoardCell boardCell in this.board)
        {
            if (!boardCell.IsOccupied) continue;
            if (col != null && boardCell.X == col) numberOfOccupiedCells++;
            else if (row != null && boardCell.Y == row) numberOfOccupiedCells++;
        }

        return numberOfOccupiedCells == BoardConstant.boardSize;
    }

    private void Clear(int? col = null, int? row = null)
    {
        foreach (BoardCell boardCell in this.board)
        {
            if (col != null && boardCell.X == col) boardCell.Clear();
            else if (row != null && boardCell.Y == row) boardCell.Clear();
        }
    }

    private List<int> CreateIntList(int size)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < size; i++)
        {
            list.Add(i);
        }

        return list;
    }
}