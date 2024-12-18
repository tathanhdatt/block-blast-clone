using Dt.Attribute;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [Title("Container")]
    [SerializeField, Required]
    private CellGraphicContainer graphicContainer;

    [Title("Graphic")]
    [SerializeField, Required]
    private Image activeGraphic;

    [SerializeField, Required]
    private Image defaultGraphic;

    [Title("Read Only Attributes")]
    [SerializeField, ReadOnly]
    private bool isStatic;

    public RectTransform RectTransform => transform as RectTransform;
    public Image ActiveGraphic => this.activeGraphic;
    public Image DefaultGraphic => this.defaultGraphic;

    public bool IsStatic
    {
        get => this.isStatic;
        set => this.isStatic = value;
    }

    public void SetActiveGraphic(CellGraphicID cellGraphicID)
    {
        foreach (CellGraphicItem item in this.graphicContainer.items)
        {
            if (item.id != cellGraphicID) continue;
            this.activeGraphic.sprite = item.graphic;
            break;
        }
    }
}