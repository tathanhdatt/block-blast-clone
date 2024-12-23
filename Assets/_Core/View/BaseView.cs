using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class BaseView : MonoBehaviour
{
    private Canvas canvas;

    public virtual void Initialize()
    {
        this.canvas = GetComponent<Canvas>();
    }
    public virtual void Show()
    {
        this.canvas.enabled = true;
        this.gameObject.SetActive(true);
    }
    
    public virtual void Hide()
    {
        this.canvas.enabled = false;
        this.gameObject.SetActive(false);
    }
}
