using System;
using UnityEngine;

public class GameViewPresenter : BaseViewPresenter, IDisposable
{
    private GameView gameView;

    public GameViewPresenter(GamePresenter gamePresenter, Transform transform)
        : base(gamePresenter, transform)
    {
        Messenger.AddListener<int>(Message.scoreChanged, OnScoreChangedHandler);
    }

    private void OnScoreChangedHandler(int score)
    {
        this.gameView.UpdateScore(score);
    }

    protected override void AddViews()
    {
        this.gameView = AddView<GameView>();
    }

    public void Dispose()
    {
        Messenger.RemoveListener<int>(Message.scoreChanged, OnScoreChangedHandler);
    }
}