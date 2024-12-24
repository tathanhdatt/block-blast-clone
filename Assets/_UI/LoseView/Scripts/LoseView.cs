using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using Dt.Extension;
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
        await ScaleUpButton();
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

    private async UniTask ScaleUpButton()
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
}