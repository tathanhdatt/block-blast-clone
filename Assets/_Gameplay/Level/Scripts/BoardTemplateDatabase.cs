using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardTemplateDatabase
{
    private readonly Dictionary<string, BoardTemplate> data =
        new Dictionary<string, BoardTemplate>();

    public BoardTemplateDatabase()
    {
        BoardTemplate[] blocks = Resources.FindObjectsOfTypeAll<BoardTemplate>();
        foreach (BoardTemplate template in blocks)
        {
            this.data.Add(template.name, template);
        }
    }

    public BoardTemplate GetTemplate(int level)
    {
        BoardTemplate template;
        try
        {
            template = this.data[$"Level_{level}"];
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            template = this.data[$"Level_0"];
        }

        return template;
    }
}