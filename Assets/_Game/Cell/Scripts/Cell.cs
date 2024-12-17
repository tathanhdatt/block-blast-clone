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

    public RectTransform RectTransform => transform as RectTransform;
    public Image DefaultGraphic => this.defaultGraphic;
    public Image ActiveGraphic => this.activeGraphic;
}