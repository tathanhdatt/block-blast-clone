using System.Collections.Generic;
using Core.AudioService;
using Core.Service;

public class BoardCleaner
{
    private readonly BoardCell[,] board;
    private readonly CompletedEffector[] rowEffectors;
    private readonly CompletedEffector[] colEffectors;
    private readonly CompletedEffector borderEffector;

    public BoardCleaner(BoardCell[,] board,
        CompletedEffector[] rowEffectors,
        CompletedEffector[] colEffectors,
        CompletedEffector borderEffector)
    {
        this.board = board;
        this.rowEffectors = rowEffectors;
        this.colEffectors = colEffectors;
        this.borderEffector = borderEffector;
    }

    public void CleanAndPlayEffect(IList<int> rows = null, IList<int> cols = null)
    {
        rows ??= IListExtension.CreateIntList(GameConstant.boardSize);
        cols ??= IListExtension.CreateIntList(GameConstant.boardSize);
        IList<int> completedRows = this.board.GetCompletedRows(rows);
        IList<int> completedColumns = this.board.GetCompletedColumns(cols);

        this.board.ClearRows(completedRows);
        this.board.ClearColumns(completedColumns);
        PlayCleanEffectRows(completedRows);
        PlayCleanEffectColumns(completedColumns);
        if (!completedRows.IsEmpty())
        {
            CellGraphicID graphicID = this.board[0, rows[0]].GraphicID;
            PlayBorderEffect(graphicID);
        }
        else if (!completedColumns.IsEmpty())
        {
            CellGraphicID graphicID = this.board[completedColumns[0], 0].GraphicID;
            PlayBorderEffect(graphicID);
        }


        bool hasCompletedLines = !completedRows.IsEmpty() || !completedColumns.IsEmpty();
        Messenger.Broadcast(Message.hasStreak, hasCompletedLines);
        if (!hasCompletedLines) return;
        Messenger.Broadcast(Message.streak);
        if (IsEmptyBoard())
        {
            ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.unbelievable);
        }
        else
        {
            int numberOfCompletedLines = completedRows.Count + completedColumns.Count;
            Messenger.Broadcast(Message.bonus, numberOfCompletedLines);
        }
    }

    private void PlayBorderEffect(CellGraphicID graphicID)
    {
        string animationName = $"start_{this.borderEffector.GetAnimationNameFromId(graphicID)}";
        this.borderEffector.Play(animationName);
    }

    private void PlayCleanEffectRows(IList<int> rows)
    {
        foreach (int row in rows)
        {
            this.rowEffectors[row].Play(this.board[0, row].GraphicID);
        }
    }

    private void PlayCleanEffectColumns(IList<int> cols)
    {
        foreach (int col in cols)
        {
            this.colEffectors[col].Play(this.board[col, 0].GraphicID);
        }
    }

    private bool IsEmptyBoard()
    {
        foreach (BoardCell boardCell in this.board)
        {
            if (boardCell.IsOccupied)
            {
                return false;
            }
        }

        return true;
    }
}