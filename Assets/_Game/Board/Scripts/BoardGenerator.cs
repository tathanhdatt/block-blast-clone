using Dt.Attribute;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    private const int Middle = BoardConstant.boardSize / 2;

    [SerializeField, Required]
    private BoardCell prefab;

    private float CellWidth => this.prefab.RectTransform.rect.width;
    private float CellHeight => this.prefab.RectTransform.rect.height;

    private BoardCell[,] cells;
    public BoardCell[,] Cells => this.cells;

    public void Initialize()
    {
        GenerateCells();
    }

    private void GenerateCells()
    {
        this.cells = new BoardCell[BoardConstant.boardSize, BoardConstant.boardSize];
        for (int i = 0; i < BoardConstant.boardSize; i++)
        {
            for (int j = 0; j < BoardConstant.boardSize; j++)
            {
                BoardCell newCell = Instantiate(this.prefab, transform);
                newCell.gameObject.SetActive(true);
                newCell.DefaultGraphic.gameObject.SetActive(true);
                this.cells[j, i] = newCell;
                newCell.SetXY(j, i);
                SetCellPosition(j, i);
            }
        }
    }

    private void SetCellPosition(int row, int column)
    {
        Vector3 position = Vector3.zero;
        position.x = CellWidth * (row - Middle) + CellWidth / 2;
        position.y = CellHeight * (column - Middle) + CellHeight / 2;
        this.cells[row, column].RectTransform.localPosition = position;
    }
}