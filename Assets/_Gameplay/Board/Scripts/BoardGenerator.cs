using Dt.Attribute;
using Dt.Extension;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    private const int Middle = GameConstant.boardSize / 2;

    [SerializeField, ReadOnly]
    private BoardTemplate template;

    [SerializeField, Required]
    private BoardCell prefab;

    private float CellWidth => this.prefab.RectTransform.rect.width;
    private float CellHeight => this.prefab.RectTransform.rect.height;

    private BoardCell[,] cells;
    public BoardCell[,] Cells => this.cells;

    public void Initialize(BoardTemplate template)
    {
        this.template = template;
        GenerateCells();
    }

    private void GenerateCells()
    {
        ClearBoard();
        this.cells = new BoardCell[GameConstant.boardSize, GameConstant.boardSize];
        for (int i = 0; i < GameConstant.boardSize; i++)
        {
            for (int j = 0; j < GameConstant.boardSize; j++)
            {
                InstantiateCell(j, i);
            }
        }
    }

    private void InstantiateCell(int x, int y)
    {
        BoardCell newCell = Instantiate(this.prefab, transform);
        newCell.gameObject.SetActive(true);
        this.cells[x, y] = newCell;
        newCell.SetXY(x, y);
        SetCellPosition(x, y);
        bool isActive = this.template.shape[y * GameConstant.boardSize + x];
        if (isActive)
        {
            SetGraphicForActiveCell(newCell);
        }
    }

    private void SetGraphicForActiveCell(BoardCell cell)
    {
        cell.Place(true);
        cell.SetActiveGraphic(EnumExtension.GetRandom<CellGraphicID>());
        cell.ActiveGraphic.gameObject.SetActive(true);
    }

    private void SetCellPosition(int row, int column)
    {
        Vector3 position = Vector3.zero;
        position.x = CellWidth * (row - Middle) + CellWidth / 2;
        position.y = CellHeight * (column - Middle) + CellHeight / 2;
        this.cells[row, column].RectTransform.localPosition = position;
    }

    private void ClearBoard()
    {
        if (Cells == null) return;
        foreach (BoardCell boardCell in Cells)
        {
            Destroy(boardCell.gameObject);
        }
    }
}