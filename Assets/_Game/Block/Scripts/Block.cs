using System;
using System.Collections.Generic;
using DG.Tweening;
using Dt.Attribute;
using Dt.Extension;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler,
    IBeginDragHandler
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

    [SerializeField, ReadOnly]
    private List<BlockCell> blockCells = new List<BlockCell>(25);

    [SerializeField, ReadOnly]
    private List<BoardCell> hitBoardCells = new List<BoardCell>(25);

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

    public event Action<Block> OnBeginDragging;
    public event Action<Block> OnDragging;
    public event Action<Block> OnEndDragging;

    private void OnEnable()
    {
        RectTransform.localScale = Vector3.zero;
        RectTransform.DOScale(this.initialScale, 0.5f).SetEase(Ease.OutBack);
        this.graphicID = EnumExtension.GetRandom<CellGraphicID>();
    }

    public void AddBlockCell(BlockCell blockCell)
    {
        this.blockCells.Add(blockCell);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransform.DOScale(this.dragScale, 0.1f).SetEase(Ease.OutQuad);
        RectTransform.SetSiblingIndex(this.maxSiblingIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragging?.Invoke(this);
        if (Camera.main == null) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            this.gameCanvas.transform as RectTransform,
            eventData.position,
            Camera.main,
            out Vector2 localPosition);
        RectTransform.localPosition = localPosition;
    }

    public void ResetStatus()
    {
        RectTransform.DOScale(this.initialScale, 0.1f).SetEase(Ease.OutQuad);
        RectTransform.DOLocalMove(this.initialPosition, 0.1f).SetEase(Ease.OutQuad);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragging?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragging?.Invoke(this);
    }

    public List<BoardCell> HitBoardCells()
    {
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
        return this.hitBoardCells.Count == this.blockCells.Count;
    }
}