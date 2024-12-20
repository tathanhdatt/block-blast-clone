using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameOverChecker
{
    private readonly BoardCell[,] board;
    private List<Block> blocks;

    public GameOverChecker(BoardCell[,] board)
    {
        this.board = board;
    }

    public bool CheckGameOver(List<Block> blocks)
    {
        if (CanPlace(blocks))
        {
            return false;
        }

        return true;
    }

    private bool CanPlace(List<Block> blocks)
    {
        this.blocks = blocks;
        for (int i = 0; i < GameConstant.boardSize; i++)
        {
            for (int j = 0; j < GameConstant.boardSize; j++)
            {
                bool canPlace = CanPlaceBlockAt(i, j);
                if (canPlace)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanPlaceBlockAt(int originX, int originY)
    {
        foreach (Block block in this.blocks)
        {
            bool isEnoughWidth = GameConstant.boardSize - originX >= block.Width;
            bool isEnoughHeight = GameConstant.boardSize - originY >= block.Height;
            if (!isEnoughWidth || !isEnoughHeight) continue;
            bool canPlace = true;
            List<bool> prevs = new List<bool>();
            foreach (BlockCell cell in block.BlockCells)
            {
                int x = cell.X + originX;
                int y = cell.Y + originY;
                bool isOccupied = this.board[x, y].IsOccupied;
                if (!isOccupied) continue;
                canPlace = false;
                break;
            }

            if (canPlace)
            {
                return true;
            }
        }

        return false;
    }
}