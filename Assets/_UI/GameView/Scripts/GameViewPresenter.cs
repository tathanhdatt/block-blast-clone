using System;
using UnityEngine;

public class GameViewPresenter : BaseViewPresenter, IDisposable
{
    private const int MaxAnim = 3;
    private GameView gameView;
    private int streakCounter;

    public GameViewPresenter(GamePresenter gamePresenter, Transform transform)
        : base(gamePresenter, transform)
    {
        Messenger.AddListener<int>(Message.scoreChanged, OnScoreChangedHandler);
        Messenger.AddListener<bool>(Message.hasStreak, StreakOnLastPlaceHandler);
    }

    private void StreakOnLastPlaceHandler(bool hasStreak)
    {
        if (hasStreak)
        {
            this.streakCounter = Math.Clamp(this.streakCounter + 1, 1, MaxAnim);
        }
        else
        {
            if (this.streakCounter <= 0)
            {
                return;
            }

            PlayStreakDownAnim();
            this.streakCounter = Math.Clamp(this.streakCounter - 1, 0, MaxAnim);
        }

        if (this.streakCounter <= 0)
        {
            this.streakCounter = 0;
            this.gameView.StopStreakAnim();
        }
        else
        {
            PlayStreakUpAnim();
        }
    }

    private async void PlayStreakUpAnim()
    {
        string animationNameIdle = $"combo{this.streakCounter}_idle";
        if (this.streakCounter >= 3)
        {
            await this.gameView.PlayStreakAnim("combo3_blast");
            await this.gameView.PlayStreakAnim("combo3_idle");
        }
        else
        {
            string animationNameStart = $"combo{this.streakCounter}_start";
            await this.gameView.PlayStreakAnim(animationNameStart);
            await this.gameView.PlayStreakAnim(animationNameIdle, true);
        }
    }

    private async void PlayStreakDownAnim()
    {
        string animationNameDown = $"combo{this.streakCounter}_down";
        await this.gameView.PlayStreakAnim(animationNameDown);
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
        Messenger.RemoveListener<bool>(Message.hasStreak, StreakOnLastPlaceHandler);
    }
}