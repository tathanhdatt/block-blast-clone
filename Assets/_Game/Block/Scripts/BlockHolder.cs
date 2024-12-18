using DG.Tweening;
using Dt.Attribute;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockHolder : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Title("Scale")]
    [SerializeField]
    private Vector3 initialScale;

    [SerializeField]
    private Vector3 dragScale;

    [Title("Read Only Attributes")]
    [SerializeField, ReadOnly]
    private Canvas gameCanvas;

    [SerializeField, ReadOnly]
    private Vector3 initialPosition;

    [SerializeField, ReadOnly]
    private CellGraphicID graphicID;

    [SerializeField, ReadOnly]
    private int maxSiblingIndex;

    public Canvas GameCanvas
    {
        set => this.gameCanvas = value;
    }

    public Vector3 InitialPosition
    {
        set => this.initialPosition = value;
    }

    public CellGraphicID GraphicID => this.graphicID;

    public int MaxSiblingIndex
    {
        set => this.maxSiblingIndex = value;
    }

    public RectTransform RectTransform => transform as RectTransform;

    private void OnEnable()
    {
        RectTransform.localScale = Vector3.zero;
        RectTransform.DOScale(this.initialScale, 0.5f).SetEase(Ease.OutBack);
        this.graphicID = EnumExtension.GetRandom<CellGraphicID>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransform.DOScale(this.dragScale, 0.1f).SetEase(Ease.OutQuad);
        RectTransform.SetSiblingIndex(this.maxSiblingIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Camera.main == null) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.gameCanvas.transform as RectTransform,
            eventData.position,
            Camera.main,
            out Vector2 localPosition);
        RectTransform.localPosition = localPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RectTransform.DOScale(this.initialScale, 0.1f).SetEase(Ease.OutQuad);
        RectTransform.DOLocalMove(this.initialPosition, 0.1f).SetEase(Ease.OutQuad);
    }
}