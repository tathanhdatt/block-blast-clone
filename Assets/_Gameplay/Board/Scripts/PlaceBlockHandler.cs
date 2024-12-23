using System;
using System.Collections.Generic;
using Core.AudioService;
using Core.Service;

public class PlaceBlockHandler : IDisposable
{
    private List<Block> blocks;
    private List<BoardCell> boardCells = new List<BoardCell>(0);
    private readonly BoardCell[,] board;
    private IList<int> lastPaintedRows = Array.Empty<int>();
    private IList<int> lastPaintedColumns = Array.Empty<int>();

    public event Action OnRunOutOfBlock;
    public event Action<List<int>, List<int>> ClearBoard;
    public event Action CheckGameOver;

    public PlaceBlockHandler(BoardCell[,] board)
    {
        this.board = board;
    }

    public void SetBlocks(List<Block> blocks)
    {
        this.blocks = blocks;
        foreach (Block blockHolder in this.blocks)
        {
            blockHolder.OnBeginDragging += OnBeginDragHandler;
            blockHolder.OnDragging += OnDragHandler;
            blockHolder.OnEndDragging += OnEndDragHandler;
        }
    }

    private void OnBeginDragHandler(Block block)
    {
    }

    private void OnDragHandler(Block block)
    {
        foreach (BoardCell boardCell in this.boardCells)
        {
            boardCell.ResetToDefault();
        }

        this.boardCells = block.HitBoardCells();

        ResetLastPaintedColumns();
        ResetLastPaintedRows();
        if (!block.CanPlace()) return;
        foreach (BoardCell boardCell in this.boardCells)
        {
            boardCell.Hover(block.GraphicID);
        }

        PaintFullRowIfPlaced(block.GraphicID);
        PaintFullColumnIfPlaced(block.GraphicID);
    }

    private void OnEndDragHandler(Block block)
    {
        if (block.CanPlace())
        {
            ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.putIn);
            PlaceBlock();
            this.blocks.Remove(block);
            RemoveListenBlockEvent(block);
            CheckRunOutOfBlocks();
            ClearBoardIfNeeded();
            UnityEngine.Object.Destroy(block.gameObject);
            CheckGameOver?.Invoke();
        }
        else
        {
            ResetBlock(block);
        }
    }

    private void ClearBoardIfNeeded()
    {
        List<int> columns = GetHoveringColumns();
        List<int> rows = GetHoveringRows();
        ClearBoard?.Invoke(columns, rows);
    }

    private List<int> GetHoveringRows()
    {
        List<int> rows = new List<int>(5);
        foreach (BoardCell boardCell in this.boardCells)
        {
            if (!rows.Contains(boardCell.Y))
            {
                rows.Add(boardCell.Y);
            }
        }

        return rows;
    }

    private List<int> GetHoveringColumns()
    {
        List<int> columns = new List<int>(5);
        foreach (BoardCell boardCell in this.boardCells)
        {
            if (!columns.Contains(boardCell.X))
            {
                columns.Add(boardCell.X);
            }
        }

        return columns;
    }

    private void CheckRunOutOfBlocks()
    {
        if (this.blocks.IsEmpty())
        {
            OnRunOutOfBlock?.Invoke();
        }
    }

    private void PlaceBlock()
    {
        foreach (BoardCell boardCell in this.boardCells)
        {
            boardCell.Place();
        }

        Messenger.Broadcast(Message.placeBlock, this.boardCells.Count);
    }

    private void ResetBlock(Block block)
    {
        foreach (BoardCell boardCell in this.boardCells)
        {
            boardCell.ResetToDefault();
        }

        block.ResetStatus();
    }

    private void RemoveListenBlockEvent(Block block)
    {
        block.OnBeginDragging -= OnBeginDragHandler;
        block.OnDragging -= OnDragHandler;
        block.OnEndDragging -= OnEndDragHandler;
    }

    public void Dispose()
    {
        foreach (Block block in this.blocks)
        {
            RemoveListenBlockEvent(block);
        }
    }

    private void PaintFullRowIfPlaced(CellGraphicID cellGraphicID)
    {
        IList<int> rows = this.board.GetCompletedRowsIfPlaced(GetHoveringRows());
        if (rows.IsEmpty()) return;

        this.lastPaintedRows = rows;
        foreach (int row in this.lastPaintedRows)
        {
            for (int column = 0; column < GameConstant.boardSize; column++)
            {
                this.board[column, row].SetActiveGraphic(cellGraphicID);
            }
        }
    }

    private void PaintFullColumnIfPlaced(CellGraphicID cellGraphicID)
    {
        IList<int> columns = this.board.GetCompletedColumnsIfPlaced(GetHoveringColumns());
        if (columns.IsEmpty()) return;

        this.lastPaintedColumns = columns;
        foreach (int column in this.lastPaintedColumns)
        {
            for (int row = 0; row < GameConstant.boardSize; row++)
            {
                this.board[column, row].SetActiveGraphic(cellGraphicID);
            }
        }
    }

    private void ResetLastPaintedRows()
    {
        foreach (int row in this.lastPaintedRows)
        {
            for (int column = 0; column < GameConstant.boardSize; column++)
            {
                if (this.boardCells.Contains(this.board[column, row])) continue;
                this.board[column, row].ResetToDefault();
            }
        }

        if (this.lastPaintedRows.IsEmpty()) return;
        this.lastPaintedRows.Clear();
    }

    private void ResetLastPaintedColumns()
    {
        foreach (int column in this.lastPaintedColumns)
        {
            for (int row = 0; row < GameConstant.boardSize; row++)
            {
                if (this.boardCells.Contains(this.board[column, row])) continue;
                this.board[column, row].ResetToDefault();
            }
        }

        if (this.lastPaintedColumns.IsEmpty()) return;
        this.lastPaintedColumns.Clear();
    }
}