using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BaseViewPresenter
{
    protected GamePresenter GamePresenter { get; private set; }
    private readonly List<BaseView> views = new List<BaseView>();
    public Transform Transform { get; private set; }
    public bool IsShowing { get; private set; }

    protected BaseViewPresenter(GamePresenter gamePresenter, Transform transform)
    {
        GamePresenter = gamePresenter;
        Transform = transform;
    }

    public async UniTask Initialize()
    {
        AddViews();
        foreach (BaseView view in this.views)
        {
            await view.Initialize();
        }
    }

    protected abstract void AddViews();

    protected T AddView<T>() where T : BaseView
    {
        T view = Object.FindAnyObjectByType<T>();
        this.views.Add(view);
        return view;
    }

    public async UniTask Show()
    {
        IsShowing = true;
        foreach (BaseView view in this.views)
        {
            await view.Show();
        }

        OnShow();
    }

    protected virtual void OnShow()
    {
    }

    public async UniTask Hide()
    {
        IsShowing = false;
        foreach (BaseView view in this.views)
        {
            await view.Hide();
        }

        OnHide();
    }

    protected virtual void OnHide()
    {
    }
}