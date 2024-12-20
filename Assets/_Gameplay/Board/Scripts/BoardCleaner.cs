using System.Collections.Generic;
using System.Threading.Tasks;
using Core.AudioService;
using Core.Service;

public class BoardCleaner
{
    private readonly BoardCell[,] board;
    private CompletedEffector[] colEffectors;

    public BoardCleaner(BoardCell[,] board)
        // CompletedEffector[] rowEffectors,
        // CompletedEffector[] colEffectors)
    {
        this.board = board;
        // this.rowEffectors = rowEffectors;
        // this.colEffectors = colEffectors;
    }

    public async Task Clean(IList<int> rows = null, IList<int> cols = null)
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
        await Task.CompletedTask;
    }

    
}