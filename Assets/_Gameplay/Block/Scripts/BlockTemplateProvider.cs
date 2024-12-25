using System;
using System.Collections.Generic;
using Dt.Attribute;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockTemplateProvider : MonoBehaviour
{
    private const float EasyTemplateProbability = 7f / 13;
    private const float MediumTemplateProbability = 4f / 13;
    private const float HardTemplateProbability = 2f / 13;

    [SerializeField, Required]
    private List<BlockTemplate> templates;

    private readonly HashSet<BlockTemplate> spawnedTemplates = new HashSet<BlockTemplate>();

    private BoardCell[,] boardCells;

    public void SetBoard(BoardCell[,] boardCells)
    {
        this.boardCells = boardCells;
    }

    public void ShuffleTemplates()
    {
        this.templates.Shuffle();
    }

    public List<BlockTemplate> GetTemplates(int numberOfTemplates = 1)
    {
        List<BlockTemplate> blockTemplates = new List<BlockTemplate>(3);
        this.spawnedTemplates.Clear();
        for (int i = 0; i < numberOfTemplates; i++)
        {
            blockTemplates.Add(GetTemplate());
        }

        return blockTemplates;
    }

    public BlockTemplate GetTemplate()
    {
        ShuffleTemplates();
        foreach (BlockTemplate template in this.templates)
        {
            if (this.spawnedTemplates.Contains(template)) continue;
            if (!CanPlace(template)) continue;
            if (Random.Range(0f, 1f) > GetProbability(template.type)) continue;
            this.spawnedTemplates.Add(template);
            return template;
        }

        return this.templates[0];
    }

    private float GetProbability(BlockType type)
    {
        return type switch
        {
            BlockType.Easy => EasyTemplateProbability,
            BlockType.Medium => MediumTemplateProbability,
            BlockType.Hard => HardTemplateProbability,
            _ => throw new ArgumentOutOfRangeException(
                $"There is no probability for block type {type}.")
        };
    }

    private bool CanPlace(BlockTemplate template)
    {
        for (int j = 0; j <= GameConstant.boardSize - template.height; j++)
        {
            for (int i = 0; i <= GameConstant.boardSize - template.width; i++)
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