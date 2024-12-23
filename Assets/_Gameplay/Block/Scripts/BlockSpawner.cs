using System.Collections.Generic;
using Dt.Attribute;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Title("Prefab")]
    [SerializeField, Required]
    private BlockCell cellPrefab;

    [SerializeField, Required]
    private Block blockPrefab;

    [Title("Game Canvas")]
    [SerializeField, Required]
    private Canvas gameCanvas;

    [Title("Spawn Points")]
    [SerializeField]
    private List<RectTransform> spawnPoints;

    [Title("Read Only Attributes")]
    [SerializeField, ReadOnly]
    private BlockTemplate currentTemplate;

    private float CellWidth => this.cellPrefab.RectTransform.rect.width;
    private float CellHeight => this.cellPrefab.RectTransform.rect.height;

    private readonly List<Block> blockHolders = new List<Block>();

    public List<Block> Spawn(List<BlockTemplate> templates)
    {
        this.blockHolders.Clear();
        SpawnBlockByGivenTemplates(templates);
        return this.blockHolders;
    }

    private void SpawnBlockByGivenTemplates(List<BlockTemplate> templates)
    {
        int numberOfBlocks = Mathf.Clamp(templates.Count, 0, GameConstant.maxBlocks);
        for (int i = 0; i < numberOfBlocks; i++)
        {
            this.currentTemplate = templates[i];
            this.blockHolders.Add(SpawnBlock(this.spawnPoints[i]));
        }
    }

    private Block SpawnBlock(RectTransform spawnPoint)
    {
        Block block = SpawnBlockHolder(spawnPoint);
        block.Width = this.currentTemplate.width;
        block.Height = this.currentTemplate.height;
        for (int i = 0; i < this.currentTemplate.height; i++)
        {
            for (int j = 0; j < this.currentTemplate.width; j++)
            {
                int index = i * this.currentTemplate.width + j;
                bool isActiveCell = this.currentTemplate.shape[index];
                if (!isActiveCell) continue;
                Vector3 position = GetCellPosition(i, j);
                BlockCell cell = SpawnCell(block, position);
                cell.SetXY(j, i);
                block.AddBlockCell(cell);
            }
        }

        return block;
    }

    private Block SpawnBlockHolder(RectTransform spawnPoint)
    {
        Block block = Instantiate(this.blockPrefab, spawnPoint.transform);
        block.RectTransform.SetParent(transform);
        block.RectTransform.localPosition = spawnPoint.localPosition;
        block.InitialPosition = spawnPoint.localPosition;
        block.GameCanvas = this.gameCanvas;
        block.MaxSiblingIndex = this.spawnPoints.Count;
        block.Initialize(CellWidth, CellHeight);
        return block;
    }

    private Vector3 GetCellPosition(int row, int column)
    {
        Vector3 position = Vector3.zero;
        position.x = CellWidth * column;
        position.y = CellHeight * row;
        if (this.currentTemplate.width > 1)
        {
            position = AlignPosition(position);
        }

        return position;
    }

    private Vector3 AlignPosition(Vector3 position)
    {
        bool hasEvenColumn = this.currentTemplate.width % 2 == 0;
        int halfWidth = this.currentTemplate.width / 2;
        if (hasEvenColumn)
        {
            position.x -= CellWidth / 2 * halfWidth;
        }
        else
        {
            position.x -= CellWidth * halfWidth;
        }

        return position;
    }

    private BlockCell SpawnCell(Block block, Vector3 position)
    {
        BlockCell cell = Instantiate(this.cellPrefab, block.RectTransform);
        cell.gameObject.SetActive(true);
        cell.RectTransform.localPosition = position;
        cell.SetActiveGraphic(block.GraphicID);
        cell.ActiveGraphic.gameObject.SetActive(true);
        return cell;
    }
}