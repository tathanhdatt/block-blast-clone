using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using Dt.Extension;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class BaseView : MonoBehaviour
{

    private Canvas canvas;

    public virtual async UniTask Initialize()
    {
        await UniTask.CompletedTask;
        this.canvas = GetComponent<Canvas>();
    }

    public virtual async UniTask Show()
    {
        await UniTask.CompletedTask;
        gameObject.SetActive(true);
        this.canvas.enabled = true;
    }

    public virtual async UniTask Hide()
    {
        await UniTask.CompletedTask;
        this.canvas.enabled = false;
        gameObject.SetActive(false);
    }

    
}