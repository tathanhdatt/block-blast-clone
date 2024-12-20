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

    [SerializeField, ReadOnly]
    protected CellGraphicID graphicID;

    [SerializeField, ReadOnly]
    protected Sprite lastActiveSprite;

    [SerializeField, ReadOnly]
    protected CellGraphicID lastGraphicID;

    public RectTransform RectTransform => transform as RectTransform;
    public Image ActiveGraphic => this.activeGraphic;

    public void SetActiveGraphic(CellGraphicID cellGraphicID)
    {
        bool isSameGraphic = this.graphicID == cellGraphicID;
        bool hasGraphic = this.activeGraphic.sprite != null;
        if (isSameGraphic && hasGraphic) return;
        this.lastGraphicID = this.graphicID;
        this.graphicID = cellGraphicID;
        foreach (CellGraphicItem item in this.graphicContainer.items)
        {
            if (item.id != cellGraphicID) continue;
            this.lastActiveSprite = this.activeGraphic.sprite;
            this.activeGraphic.sprite = item.graphic;
            break;
        }
    }
}