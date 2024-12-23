﻿using System.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using AnimationState = Spine.AnimationState;

public class GameView : BaseView
{
    [Title("Score")]
    [SerializeField, Required]
    private TMP_Text scoreText;

    [SerializeField, Required]
    private SkeletonGraphic streakScoreEffect;

    [SerializeField, Required]
    private TMP_Text highestScore;

    private Tweener scoreTweener;
    private Tweener highestScoreTweener;

    public void UpdateScore(int score)
    {
        this.scoreTweener.Kill();
        int lastScore = int.Parse(this.scoreText.text);
        this.scoreTweener = DOTween.To(val =>
        {
            int tempScore = Mathf.FloorToInt(val);
            this.scoreText.SetText(tempScore.ToString());
        }, lastScore, score, 0.2f);
        this.scoreTweener.SetEase(Ease.OutQuad);
    }

    public async Task PlayStreakAnim(string streakAnimation, bool loop = false)
    {
        AnimationState animationState = this.streakScoreEffect.AnimationState;
        TrackEntry entry = animationState.SetAnimation(0, streakAnimation, loop);
        await Task.Delay((int)(entry.AnimationEnd * 1000));
    }

    public void UpdateHighestScore(int score)
    {
        this.highestScoreTweener.Kill();
        int lastScore = int.Parse(this.scoreText.text);
        this.highestScoreTweener = DOTween.To(val =>
        {
            int tempScore = Mathf.FloorToInt(val);
            this.highestScore.SetText(tempScore.ToString());
        }, lastScore, score, 0.2f);
        this.highestScoreTweener.SetEase(Ease.OutQuad);
    }
}