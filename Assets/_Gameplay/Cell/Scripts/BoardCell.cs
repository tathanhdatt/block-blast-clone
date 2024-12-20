using System;
using System.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Dt.Attribute;
using Dt.Extension;
using Spine;
using Spine.Unity;
using UnityEngine;

public class BoardCell : Cell
{
    [Title("Board Cell")]
    [SerializeField, Required]
    private SkeletonGraphic skeletonGraphic;

    [SerializeField]
    private float disappearTime = 0.5f;

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

    private void OnEnable()
    {
        this.skeletonGraphic.AnimationState.Complete += AnimationStateOnComplete;
    }

    private void OnDisable()
    {
        this.skeletonGraphic.AnimationState.Complete -= AnimationStateOnComplete;
    }

    private void AnimationStateOnComplete(TrackEntry trackentry)
    {
        this.skeletonGraphic.gameObject.SetActive(false);
    }

    public void SetXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void ResetToDefault()
    {
        ResetActiveGraphicToLastSprite();
        if (this.isOccupied) return;
        this.activeGraphic.sprite = null;
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
        this.lastActiveSprite = null;
    }

    public async void Clear()
    {
        if (!this.isOccupied) return;
        this.isOccupied = false;
        PlayClearEffect();
        await ScaleActiveImageToZero();
        ResetToDefault();
    }

    private Task ScaleActiveImageToZero()
    {
        TweenerCore<Vector3, Vector3, VectorOptions> tweenScale =
            this.activeGraphic.transform.DOScale(0, this.disappearTime);
        tweenScale.SetEase(Ease.OutCirc);
        return tweenScale.AsyncWaitForCompletion();
    }

    private void ResetActiveGraphicToLastSprite()
    {
        if (this.lastActiveSprite == null) return;
        (this.graphicID, this.lastGraphicID) = (this.lastGraphicID, this.graphicID);
        (this.activeGraphic.sprite, this.lastActiveSprite) =
            (this.lastActiveSprite, this.activeGraphic.sprite);
    }

    private void PlayClearEffect()
    {
        this.skeletonGraphic.gameObject.SetActive(true);
        this.skeletonGraphic.AnimationState.SetAnimation(0, GetAnimationName(), false);
    }

    private string GetAnimationName()
    {
        return this.graphicID switch
        {
            CellGraphicID.Blue => "cyan",
            CellGraphicID.Cyan => "cyan",
            CellGraphicID.Green => "green",
            CellGraphicID.Orange => "orange",
            CellGraphicID.Red => "orange",
            CellGraphicID.Purple => "violet",
            CellGraphicID.Yellow => "yellow",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}