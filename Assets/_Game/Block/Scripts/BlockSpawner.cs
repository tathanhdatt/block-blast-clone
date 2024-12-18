using System.Collections.Generic;
using Dt.Attribute;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [Title("Template")]
    [SerializeField]
    private List<BlockTemplate> templates;

    [SerializeField, Required]
    private Cell cellPrefab;

    [SerializeField, Required]
    private BlockHolder blockHolderPrefab;

    private float CellWidth => this.cellPrefab.RectTransform.rect.width;
    private float CellHeight => this.cellPrefab.RectTransform.rect.height;

    [Title("Game Canvas")]
    [SerializeField, Required]
    private Canvas gameCanvas;

    [Title("Spawn Points")]
    [SerializeField]
    private List<RectTransform> spawnPoints;

    [Title("Read Only Attributes")]
    [SerializeField, ReadOnly]
    private BlockTemplate currentTemplate;

    private void Awake()
    {
        SpawnBlockAtSpawnPoints();
    }

    public void SpawnBlockAtSpawnPoints()
    {
        foreach (RectTransform spawnPoint in this.spawnPoints)
        {
            SpawnBlock(spawnPoint);
        }
    }

    private void SpawnBlock(RectTransform spawnPoint)
    {
        this.currentTemplate = GetRandomTemplate();
        BlockHolder blockHolder = SpawnBlockHolder(spawnPoint);
        for (int i = 0; i < this.currentTemplate.height; i++)
        {
            for (int j = 0; j < this.currentTemplate.width; j++)
            {
                Vector3 position = GetCellPosition(j, i);
                Cell cell = SpawnCell(blockHolder, position);
                int index = i * this.currentTemplate.width + j;
                bool isActiveCell = this.currentTemplate.shape[index];
                cell.gameObject.SetActive(isActiveCell);
            }
        }
    }

    private BlockTemplate GetRandomTemplate()
    {
        int randomIndex = Random.Range(0, this.templates.Count);
        return this.templates[randomIndex];
    }

    private BlockHolder SpawnBlockHolder(RectTransform spawnPoint)
    {
        BlockHolder blockHolder = Instantiate(this.blockHolderPrefab, spawnPoint.transform);
        blockHolder.RectTransform.SetParent(transform);
        blockHolder.RectTransform.localPosition = spawnPoint.localPosition;
        blockHolder.InitialPosition = spawnPoint.localPosition;
        blockHolder.GameCanvas = this.gameCanvas;
        blockHolder.MaxSiblingIndex = this.spawnPoints.Count;
        return blockHolder;
    }

    private Vector3 GetCellPosition(int row, int column)
    {
        Vector3 position = Vector3.zero;
        position.x = CellWidth * row;
        position.y = CellHeight * column;
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

    private Cell SpawnCell(BlockHolder blockHolder, Vector3 position)
    {
        Cell cell = Instantiate(this.cellPrefab, blockHolder.RectTransform);
        cell.RectTransform.localPosition = position;
        cell.SetActiveGraphic(blockHolder.GraphicID);
        cell.ActiveGraphic.gameObject.SetActive(true);
        cell.IsStatic = false;
        return cell;
    }
}