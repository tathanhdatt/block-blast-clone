using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class BaseView : MonoBehaviour
{
    private Canvas canvas;
    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        canvas = GetComponent<Canvas>();
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
