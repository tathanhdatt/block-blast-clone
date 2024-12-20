using System;
using System.Collections.Generic;
using Core.AudioService;
using Core.Service;

public class PlaceBlockHandler : IDisposable
{
    private List<Block> blocks;
    private List<BoardCell> boardCells = new List<BoardCell>(0);

    public event Action OnRunOutOfBlock;
    public event Action<List<int>, List<int>> ClearBoard;
    public event Action CheckGameOver;

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
        foreach (BoardCell boardCell in this.boardCells)
        {
            boardCell.Hover(block.GraphicID);
        }
    }

    private void OnEndDragHandler(Block block)
    {
        if (block.CanPlace())
        {
            ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.putIn);
            PlaceBlock();
            ClearBoardIfNeeded();
            RemoveListenBlockEvent(block);
            this.blocks.Remove(block);
            UnityEngine.Object.Destroy(block.gameObject);
            CheckGameOver?.Invoke();
        }
        else
        {
            ResetBlock(block);
        }

        CheckRunOutOfBlocks();
    }

    private void ClearBoardIfNeeded()
    {
        List<int> x = new List<int>(5);
        List<int> y = new List<int>(5);
        foreach (BoardCell boardCell in this.boardCells)
        {
            if (!x.Contains(boardCell.X))
            {
                x.Add(boardCell.X);
            }

            if (!y.Contains(boardCell.Y))
            {
                y.Add(boardCell.Y);
            }
        }
        ClearBoard?.Invoke(x, y);
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
}