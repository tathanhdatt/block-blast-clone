using System.Collections.Generic;
using Dt.Attribute;
using UnityEngine;

public class RandomBlockTemplateProvider : MonoBehaviour
{
    [SerializeField, Required]
    private List<BlockTemplate> templates;

    public BlockTemplate GetTemplate()
    {
        return this.templates.RandomItem();
    }
}