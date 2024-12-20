using System;
using Dt.Attribute;
using Spine;
using Spine.Unity;
using UnityEngine;

public class CompletedEffector : MonoBehaviour, IDisposable
{
    [SerializeField, Required]
    private SkeletonGraphic skeletonGraphic;

    public void Initialize()
    {
        this.skeletonGraphic.AnimationState.Complete += AnimationStateOnCompleteHandler;
    }

    private void AnimationStateOnCompleteHandler(TrackEntry trackentry)
    {
        gameObject.SetActive(false);
    }

    public void Play(CellGraphicID graphicID)
    {
        gameObject.SetActive(true);
        string animationName = GetAnimationNameFromId(graphicID);
        if (string.IsNullOrEmpty(animationName)) return;
        this.skeletonGraphic.AnimationState.SetAnimation(0, animationName, false);
    }

    private string GetAnimationNameFromId(CellGraphicID graphicID)
    {
        return graphicID switch
        {
            CellGraphicID.Blue => "blue",
            CellGraphicID.Cyan => "cyan",
            CellGraphicID.Green => "green",
            CellGraphicID.Orange => "orange",
            CellGraphicID.Red => "red",
            CellGraphicID.Purple => "violet",
            CellGraphicID.Yellow => "yellow",
            _ => string.Empty
        };
    }

    public void Dispose()
    {
        this.skeletonGraphic.AnimationState.Complete -= AnimationStateOnCompleteHandler;
    }
}