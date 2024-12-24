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

    [SerializeField, Required]
    private CompletedEffector completedEffectorPrefab;

    [SerializeField, Required]
    private CompletedEffector borderEffector;

    public CompletedEffector BorderEffector => this.borderEffector;

    private float CellWidth => this.prefab.RectTransform.rect.width;
    private float CellHeight => this.prefab.RectTransform.rect.height;

    public BoardCell[,] Cells { get; private set; }

    public CompletedEffector[] RowEffectors { get; } =
        new CompletedEffector[GameConstant.boardSize];

    public CompletedEffector[] ColumnEffectors { get; } =
        new CompletedEffector[GameConstant.boardSize];

    public void Initialize(BoardTemplate template)
    {
        this.template = template;
        this.borderEffector.Initialize();
        GenerateCells();
        GenerateCompletedEffector();
    }

    public void Terminate()
    {
        ClearBoard();
    }

    private void GenerateCompletedEffector()
    {
        for (int i = 0; i < GameConstant.boardSize; i++)
        {
            CompletedEffector effector =
                Instantiate(this.completedEffectorPrefab, transform);
            Vector3 position = Cells[0, i].transform.position;
            position.x = 0;
            effector.transform.position = position;
            effector.gameObject.SetActive(false);
            RowEffectors[i] = effector;
        }

        for (int i = 0; i < GameConstant.boardSize; i++)
        {
            CompletedEffector effector =
                Instantiate(this.completedEffectorPrefab, transform);
            Vector3 position = Cells[i, 0].transform.localPosition;
            position.y = 0;
            effector.transform.localPosition = position;
            effector.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            effector.gameObject.SetActive(false);
            effector.Initialize();
            ColumnEffectors[i] = effector;
        }
    }

    private void GenerateCells()
    {
        ClearBoard();
        this.Cells = new BoardCell[GameConstant.boardSize, GameConstant.boardSize];
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
        this.Cells[x, y] = newCell;
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
        this.Cells[row, column].RectTransform.localPosition = position;
    }

    private void ClearBoard()
    {
        if (Cells == null) return;
        foreach (BoardCell boardCell in Cells)
        {
            if (boardCell != null)
            {
                Destroy(boardCell.gameObject);
            }
        }
    }
}