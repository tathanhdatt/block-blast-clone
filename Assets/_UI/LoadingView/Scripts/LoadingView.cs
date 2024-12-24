using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using Dt.Extension;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : BaseView
{
    [SerializeField, Required]
    private Image fadeBackground;

    [SerializeField, Required]
    private Image loadingImage;

    public override async UniTask Show()
    {
        await base.Show();
        await FadeOut(0.8f);
        await this.loadingImage.DOFillAmount(0.2f, 0.2f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(2f);
        await this.loadingImage.DOFillAmount(0.8f, 0.4f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(0.1f);
        await this.loadingImage.DOFillAmount(1, 0.1f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
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
}