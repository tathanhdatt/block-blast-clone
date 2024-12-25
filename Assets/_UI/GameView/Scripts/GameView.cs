using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using Dt.Extension;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AnimationState = Spine.AnimationState;

public class GameView : BaseView
{
    [Title("Background")]
    [SerializeField, Required]
    private Image fadeBackground;

    [Title("Score")]
    [SerializeField, Required]
    private TMP_Text scoreText;

    [SerializeField, Required]
    private SkeletonGraphic streakScoreEffect;

    [SerializeField, Required]
    private TMP_Text highestScore;

    private Tweener scoreTweener;
    private Tweener highestScoreTweener;

    public override async UniTask Show()
    {
        await base.Show();
        await FadeOut(0.8f);
    }

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

    public override async UniTask Hide()
    {
        await FadeIn(1f);
        await base.Hide();
    }

    private async UniTask FadeIn(float duration)
    {
        this.fadeBackground.gameObject.SetActive(true);
        this.fadeBackground.color = Color.black.SetAlpha(0);
        await this.fadeBackground.DOFade(1, duration)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
    }

    private async UniTask FadeOut(float duration)
    {
        this.fadeBackground.color = Color.black.SetAlpha(1);
        await this.fadeBackground.DOFade(0, duration)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
        this.fadeBackground.gameObject.SetActive(false);
    }

    public void SetScore(int score)
    {
        this.scoreText.SetText(score.ToString());
    }

    public void SetHighestScore(int score)
    {
        this.highestScore.SetText(score.ToString());
    }
}