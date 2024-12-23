using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : BaseView
{
    [SerializeField, Required]
    private Image loadingImage;

    public override async UniTask Show()
    {
        await base.Show();
        await this.loadingImage.DOFillAmount(0.2f, 0.2f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(2f);
        await this.loadingImage.DOFillAmount(0.8f, 0.4f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
        await UniTask.WaitForSeconds(0.1f);
        await this.loadingImage.DOFillAmount(1, 0.1f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
    }
}