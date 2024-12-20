using System.Collections.Generic;
using Dt.Attribute;
using UnityEngine;

public class BlockTemplateProvider : MonoBehaviour
{
    [SerializeField, Required]
    private PlaceableBlockTemplateProvider placeableBlockTemplateProvider;

    [SerializeField, Required]
    private RandomBlockTemplateProvider randomBlockTemplateProvider;

    public void SetBoard(BoardCell[,] board)
    {
        this.placeableBlockTemplateProvider.SetBoardCells(board);
    }

    public void ShuffleTemplates()
    {
        this.placeableBlockTemplateProvider.ShuffleTemplates();
    }

    public List<BlockTemplate> GetRandomBlockTemplates(int numBlocks = 1)
    {
        List<BlockTemplate> templates = new List<BlockTemplate>(GameConstant.maxBlocks);
        for (int i = 0; i < numBlocks; i++)
        {
            templates.Add(this.randomBlockTemplateProvider.GetTemplate());
        }

        return templates;
    }

    public List<BlockTemplate> GetPlaceableBlockTemplates(int numBlocks = 1)
    {
        List<BlockTemplate> templates = new List<BlockTemplate>(GameConstant.maxBlocks);
        for (int i = 0; i < numBlocks; i++)
        {
            BlockTemplate template = this.placeableBlockTemplateProvider.GetTemplate();
            if (template == null) break;
            templates.Add(template);
        }

        return templates;
    }
}