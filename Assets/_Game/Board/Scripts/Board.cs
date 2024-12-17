using System;
using Dt.Attribute;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField, Required]
    private Cell prefab;

    private float CellWidth => this.prefab.RectTransform.rect.width;
    private float CellHeight => this.prefab.RectTransform.rect.height;

    private Cell[,] cells;
    

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        GenerateCells();
    }

    private void GenerateCells()
    {
        this.cells = new Cell[BoardConstant.boardSize, BoardConstant.boardSize];
        for (int i = 0; i < BoardConstant.boardSize; i++)
        {
            for (int j = 0; j < BoardConstant.boardSize; j++)
            {
                Cell newCell = Instantiate(this.prefab, this.transform);
                this.cells[i, j] = newCell;
                SetCellPosition(i, j);
            }
        }
    }

    private void SetCellPosition(int row, int column)
    {
        const int middle = BoardConstant.boardSize / 2;
        Vector3 position = Vector3.zero;
        position.x = CellWidth * (row - middle) + CellWidth / 2;
        position.y = CellHeight * (column - middle) + CellHeight / 2;
        this.cells[row, column].RectTransform.localPosition = position;
    }
}