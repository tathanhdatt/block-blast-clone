using System;
using System.Collections.Generic;
using Core.Game;
using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    private readonly Dictionary<Type, BaseViewPresenter> presenters =
        new Dictionary<Type, BaseViewPresenter>();

    private GameManager manager;

    public void Enter(GameManager gameManager)
    {
        this.manager = gameManager;
    }

    public void InitialViewPresenters()
    {
        foreach (BaseViewPresenter presenter in this.presenters.Values)
        {
            presenter.Initialize();
        }
    }

    public void AddPresenter(BaseViewPresenter presenter)
    {
        this.presenters.Add(presenter.GetType(), presenter);
    }

    public T GetViewPresenter<T>() where T : BaseViewPresenter
    {
        Type type = typeof(T);
        return (T)this.presenters[type];
    }
}