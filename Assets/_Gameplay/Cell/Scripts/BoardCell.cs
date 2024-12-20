using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Dt.Attribute;
using Dt.Extension;
using UnityEngine;

public class BoardCell : Cell
{
    [Title("Board Cell")]
    [SerializeField, ReadOnly]
    private int x;

    [SerializeField, ReadOnly]
    private int y;

    [SerializeField, ReadOnly]
    private bool isHovering;

    [SerializeField, ReadOnly]
    private bool isOccupied;

    public bool IsHovering => this.isHovering;
    public bool IsOccupied => this.isOccupied;
    public int X => this.x;
    public int Y => this.y;

    public void SetXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void ResetToDefault()
    {
        ResetActiveGraphicToLastSprite();
        if (this.isOccupied) return;
        this.activeGraphic.gameObject.SetActive(false);
        this.activeGraphic.color = Color.white;
        this.isHovering = false;
        this.activeGraphic.transform.localScale = Vector3.one;
    }

    public void Hover(CellGraphicID cellGraphicID)
    {
        if (this.isOccupied) return;
        this.activeGraphic.gameObject.SetActive(true);
        this.activeGraphic.color = Color.white.SetAlpha(0.6f);
        this.isHovering = true;
        SetActiveGraphic(cellGraphicID);
    }

    public void Place(bool force = false)
    {
        if (!force && !this.isHovering) return;
        this.isOccupied = true;
        this.activeGraphic.gameObject.SetActive(true);
        this.activeGraphic.color = Color.white;
    }

    public async void Clear()
    {
        if (!this.isOccupied) return;
        this.isOccupied = false;
        await ScaleActiveImageToZero();
        ResetToDefault();
    }

    private Task ScaleActiveImageToZero()
    {
        TweenerCore<Vector3, Vector3, VectorOptions> tweenScale =
            this.activeGraphic.transform.DOScale(0, 0.1f);
        tweenScale.SetEase(Ease.OutBounce);
        return tweenScale.AsyncWaitForCompletion();
    }

    private void ResetActiveGraphicToLastSprite()
    {
        if (this.lastActiveSprite == null) return;
        this.graphicID = this.lastGraphicID;
        this.activeGraphic.sprite = this.lastActiveSprite;
    }
}