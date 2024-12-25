using System;
using System.Globalization;
using Core.AudioService;
using Core.Service;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using Dt.Extension;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseView : BaseView
{
    [Title("Background")]
    [SerializeField, Required]
    private Image background;

    [SerializeField, Required]
    private Image fadeBackground;

    [Title("Button")]
    [SerializeField, Required]
    private Button replayButton;

    [Title("Effect")]
    [SerializeField, Required]
    private SkeletonGraphic crown;

    [SerializeField, Required]
    private ParticleSystem confetti;

    [Title("Score")]
    [SerializeField, Required]
    private TMP_Text score;

    public event Action OnClickReplay;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        this.replayButton.onClick.AddListener(() => OnClickReplay?.Invoke());
    }

    public override async UniTask Show()
    {
        ResetAlphaBackground();
        ResetScaleButton();
        await base.Show();
        await FadeInBackground();
    }

    public override async UniTask Hide()
    {
        ResetAlphaFadeBackground();
        EnableFadeBackground();
        await FadeInFadeBackground();
        await base.Hide();
        DisableFadeBackground();
    }

    private async UniTask FadeInBackground()
    {
        Tweener tweener = this.background.DOFade(0.6f, 0.2f).SetEase(Ease.OutQuad);
        await tweener.AsyncWaitForCompletion();
    }

    public async UniTask ScaleUpButton()
    {
        Tweener tweener = this.replayButton.transform
            .DOScale(1, 0.4f).SetEase(Ease.OutBack);
        await tweener.AsyncWaitForCompletion();
    }

    private void ResetScaleButton()
    {
        this.replayButton.transform.localScale = Vector3.zero;
    }

    private void ResetAlphaBackground()
    {
        this.background.color = Color.black.SetAlpha(0);
    }

    private void ResetAlphaFadeBackground()
    {
        this.fadeBackground.color = Color.black.SetAlpha(0);
    }

    private void EnableFadeBackground()
    {
        this.fadeBackground.gameObject.SetActive(true);
    }

    private void DisableFadeBackground()
    {
        this.fadeBackground.gameObject.SetActive(false);
    }

    private async UniTask FadeInFadeBackground()
    {
        Tweener tweener = this.fadeBackground.DOFade(1, 1).SetEase(Ease.OutQuad);
        await tweener.AsyncWaitForCompletion();
    }

    public void EnableCrown()
    {
        this.crown.gameObject.SetActive(true);
        this.crown.AnimationState.SetAnimation(0, "animation", false);
    }

    public void DisableCrown()
    {
        this.crown.gameObject.SetActive(false);
    }

    public void PlayConfetti()
    {
        this.confetti.Play();
    }

    public async UniTask ShowScore(int score)
    {
        Tweener tweener = DOTween.To(val => SetScore((int)val),
            0, score, 0.4f);
        tweener.SetEase(Ease.OutQuad);
        await tweener.AsyncWaitForCompletion();
    }

    private void SetScore(int score)
    {
        this.score.SetText(score.ToString());
    }
}