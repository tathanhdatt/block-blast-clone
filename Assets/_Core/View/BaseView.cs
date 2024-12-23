using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using Dt.Extension;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class BaseView : MonoBehaviour
{
    [SerializeField, Required]
    private Image fadeBackground;

    private Canvas canvas;

    public virtual async UniTask Initialize()
    {
        await UniTask.CompletedTask;
        this.canvas = GetComponent<Canvas>();
    }

    public virtual async UniTask Show()
    {
        gameObject.SetActive(true);
        this.canvas.enabled = true;
        await FadeOut(0.8f);
    }

    public virtual async UniTask Hide()
    {
        await FadeIn(1f);
        this.canvas.enabled = false;
        gameObject.SetActive(false);
    }

    protected async UniTask FadeIn(float duration)
    {
        this.fadeBackground.gameObject.SetActive(true);
        this.fadeBackground.color = Color.black.SetAlpha(0);
        await this.fadeBackground.DOFade(1, duration)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
    }

    protected async UniTask FadeOut(float duration)
    {
        this.fadeBackground.color = Color.black.SetAlpha(1);
        await this.fadeBackground.DOFade(0, duration)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
        this.fadeBackground.gameObject.SetActive(false);
    }
}