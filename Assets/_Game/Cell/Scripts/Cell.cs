using Dt.Attribute;
using UnityEngine;
using UnityEngine.UI;

public abstract class Cell : MonoBehaviour
{
    [Title("Container")]
    [SerializeField, Required]
    private CellGraphicContainer graphicContainer;

    [Title("Graphic")]
    [SerializeField, Required]
    protected Image activeGraphic;

    [SerializeField, Required]
    protected Image defaultGraphic;

    [SerializeField, ReadOnly]
    protected CellGraphicID graphicID;

    public RectTransform RectTransform => transform as RectTransform;
    public Image ActiveGraphic => this.activeGraphic;
    public Image DefaultGraphic => this.defaultGraphic;

    public void SetActiveGraphic(CellGraphicID cellGraphicID)
    {
        this.graphicID = cellGraphicID;
        foreach (CellGraphicItem item in this.graphicContainer.items)
        {
            if (item.id != cellGraphicID) continue;
            this.activeGraphic.sprite = item.graphic;
            break;
        }
    }
}