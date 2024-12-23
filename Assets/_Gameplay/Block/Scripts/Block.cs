using System;
using System.Collections.Generic;
using Core.AudioService;
using Core.Service;
using DG.Tweening;
using Dt.Attribute;
using Dt.Extension;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler,
    IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [Title("Scale")]
    [SerializeField]
    private Vector3 initialScale;

    [SerializeField]
    private Vector3 dragScale;

    [SerializeField]
    private Vector2 dragOffset;

    [Title("Read Only Attributes")]
    [SerializeField, ReadOnly]
    private int width;

    [SerializeField, ReadOnly]
    private int height;

    [SerializeField, ReadOnly]
    private Canvas gameCanvas;

    [SerializeField, ReadOnly]
    private Vector3 initialPosition;

    [SerializeField, ReadOnly]
    private CellGraphicID graphicID;

    [SerializeField, ReadOnly]
    private int maxSiblingIndex;

    [SerializeField, ReadOnly]
    private bool isDragging;

    [SerializeField, ReadOnly]
    private List<BlockCell> blockCells = new List<BlockCell>(25);

    [SerializeField, ReadOnly]
    private List<BoardCell> hitBoardCells = new List<BoardCell>(25);

    [SerializeField, ReadOnly]
    private Vector2 lastPointerPosition;

    [SerializeField, ReadOnly]
    private float cellWidth;

    [SerializeField, ReadOnly]
    private float cellHeight;

    [SerializeField, ReadOnly]
    private bool canHit;

    public int Width
    {
        get => this.width;
        set => this.width = value;
    }

    public int Height
    {
        get => this.height;
        set => this.height = value;
    }

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
    public List<BlockCell> BlockCells => this.blockCells;

    public event Action<Block> OnBeginDragging;
    public event Action<Block> OnDragging;
    public event Action<Block> OnEndDragging;

    public void Initialize(float cellWidth, float cellHeight)
    {
        RectTransform.localScale = Vector3.zero;
        RectTransform.DOScale(this.initialScale, 0.5f).SetEase(Ease.OutBack);
        this.graphicID = EnumExtension.GetRandom<CellGraphicID>();
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
    }

    public void AddBlockCell(BlockCell blockCell)
    {
        this.blockCells.Add(blockCell);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.takeUp);
        RectTransform.DOScale(this.dragScale, 0.1f).SetEase(Ease.OutQuad);
        RectTransform.SetSiblingIndex(this.maxSiblingIndex);
        RectTransform.localPosition += new Vector3(this.dragOffset.x, this.dragOffset.y, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.isDragging = true;
        this.lastPointerPosition = GetLocalPositionInCanvasRect(eventData.position);
        OnBeginDragging?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragging?.Invoke(this);
        if (Camera.main == null) return;
        Vector2 localPosition = GetLocalPositionInCanvasRect(eventData.position);
        RectTransform.localPosition = localPosition + this.dragOffset;
        float diffX = Mathf.Abs(localPosition.x - this.lastPointerPosition.x);
        float diffY = Mathf.Abs(localPosition.y - this.lastPointerPosition.y);
        if (diffX > this.cellWidth / 2 || diffY > this.cellHeight / 2)
        {
            this.canHit = true;
            this.lastPointerPosition = localPosition;
        }
        else
        {
            this.canHit = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.isDragging = false;
        OnEndDragging?.Invoke(this);
    }

    public void ResetStatus()
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.takeDown);
        RectTransform.DOScale(this.initialScale, 0.1f).SetEase(Ease.OutQuad);
        RectTransform.DOLocalMove(this.initialPosition, 0.1f).SetEase(Ease.OutQuad);
    }


    public List<BoardCell> HitBoardCells()
    {
        if (!this.canHit) return this.hitBoardCells;
        this.hitBoardCells.Clear();
        foreach (BlockCell blockCell in this.blockCells)
        {
            BoardCell boardCell = blockCell.Hit();
            if (boardCell == null) continue;
            if (this.hitBoardCells.Contains(boardCell)) continue;
            if (boardCell.IsOccupied) continue;
            this.hitBoardCells.Add(boardCell);
        }

        return this.hitBoardCells;
    }

    public bool CanPlace()
    {
        if (this.hitBoardCells.IsEmpty()) return false;
        return this.hitBoardCells.Count == this.blockCells.Count;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.isDragging) return;
        ResetStatus();
    }

    private Vector2 GetLocalPositionInCanvasRect(Vector2 screenPoint)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.gameCanvas.transform as RectTransform,
            screenPoint,
            Camera.main,
            out Vector2 localPosition);
        return localPosition;
    }
}