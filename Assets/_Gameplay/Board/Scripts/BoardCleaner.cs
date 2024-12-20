using System.Collections.Generic;
using Core.AudioService;
using Core.Service;

public class BoardCleaner
{
    private readonly BoardCell[,] board;
    private readonly CompletedEffector[] rowEffectors;
    private readonly CompletedEffector[] colEffectors;

    public BoardCleaner(BoardCell[,] board,
        CompletedEffector[] rowEffectors,
        CompletedEffector[] colEffectors)
    {
        this.board = board;
        this.rowEffectors = rowEffectors;
        this.colEffectors = colEffectors;
    }

    public void CleanAndPlayEffect(IList<int> rows = null, IList<int> cols = null)
    {
        rows ??= IListExtension.CreateIntList(GameConstant.boardSize);
        cols ??= IListExtension.CreateIntList(GameConstant.boardSize);
        IList<int> completedRows = this.board.GetCompletedRows(rows);
        IList<int> completedColumns = this.board.GetCompletedColumns(cols);
        if (!completedRows.IsEmpty() || !completedColumns.IsEmpty())
        {
            ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.streak1);
        }

        this.board.ClearRows(completedRows);
        this.board.ClearColumns(completedColumns);
        PlayCleanEffectRows(completedRows);
        PlayCleanEffectColumns(completedColumns);
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
}