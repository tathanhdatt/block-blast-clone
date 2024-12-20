using System.Collections.Generic;
using Dt.Attribute;
using UnityEngine;

public class TemplatePlaceableProvider : MonoBehaviour
{
    [SerializeField, Required]
    private List<BlockTemplate> templates;

    [SerializeField, ReadOnly]
    private List<BlockTemplate> usedTemplates = new List<BlockTemplate>(30);

    private BoardCell[,] boardCells;

    public void SetBoardCells(BoardCell[,] boardCells)
    {
        this.boardCells = boardCells;
    }

    public void ShuffleTemplates()
    {
        this.templates.Shuffle();
    }

    public BlockTemplate GetTemplate()
    {
        if (this.templates.Count == 0)
        {
            this.templates = this.usedTemplates;
            this.usedTemplates = new List<BlockTemplate>(30);
        }

        foreach (BlockTemplate template in this.templates)
        {
            if (!CanPlace(template)) continue;
            this.templates.Remove(template);
            this.usedTemplates.Add(template);
            return template;
        }

        foreach (BlockTemplate template in this.usedTemplates)
        {
            if (!CanPlace(template)) continue;
            return template;
        }

        return null;
    }

    private bool CanPlace(BlockTemplate template)
    {
        for (int i = 0; i <= BoardConstant.boardSize - template.width; i++)
        {
            for (int j = 0; j <= BoardConstant.boardSize - template.height; j++)
            {
                if (CanPlaceAt(i, j, template))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanPlaceAt(int originX, int originY, BlockTemplate template)
    {
        for (int i = 0; i < template.height; i++)
        {
            for (int j = 0; j < template.width; j++)
            {
                int index = i * template.width + j;
                bool isActiveCell = template.shape[index];
                if (!isActiveCell) continue;
                int x = originX + j;
                int y = originY + i;
                if (this.boardCells[x, y].IsOccupied)
                {
                    return false;
                }
            }
        }

        return true;
    }
}