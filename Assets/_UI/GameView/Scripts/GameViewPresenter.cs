using System;
using Core.AudioService;
using Core.Service;
using UnityEngine;

public class GameViewPresenter : BaseViewPresenter, IDisposable
{
    private const int MaxAnim = 3;
    private GameView gameView;
    private int streakCounter;
    private int lastHighestScore;
    private bool isPlayedNewHighScoreSound;

    public GameViewPresenter(GamePresenter gamePresenter, Transform transform)
        : base(gamePresenter, transform)
    {
        this.streakCounter = 1;
        Messenger.AddListener<int>(Message.scoreChanged, OnScoreChangedHandler);
        Messenger.AddListener<bool>(Message.hasStreak, StreakOnLastPlaceHandler);
        Messenger.AddListener(Message.replay, OnReplayHandler);
    }


    protected override void OnShow()
    {
        base.OnShow();
        this.lastHighestScore = PlayerPrefs.GetInt(PlayerPrefsVar.highestScore, 0);
        this.gameView.UpdateHighestScore(this.lastHighestScore);
    }

    private void StreakOnLastPlaceHandler(bool hasStreak)
    {
        if (hasStreak)
        {
            this.streakCounter = Math.Clamp(this.streakCounter + 1, 2, MaxAnim);
        }
        else
        {
            PlayStreakDownAnim();
            this.streakCounter = Math.Clamp(this.streakCounter - 1, 1, MaxAnim);
        }

        PlayStreakUpAnim();
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
        if (this.lastHighestScore > score) return;
        if (!this.isPlayedNewHighScoreSound)
        {
            ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.newHighScoreInGame);
            this.isPlayedNewHighScoreSound = true;
        }

        this.gameView.UpdateHighestScore(score);
    }

    private void OnReplayHandler()
    {
        this.isPlayedNewHighScoreSound = false;
        this.gameView.SetScore(0);
        this.gameView.SetHighestScore(0);
    }

    protected override void AddViews()
    {
        this.gameView = AddView<GameView>();
    }

    public void Dispose()
    {
        Messenger.RemoveListener<int>(Message.scoreChanged, OnScoreChangedHandler);
        Messenger.RemoveListener<bool>(Message.hasStreak, StreakOnLastPlaceHandler);
        Messenger.RemoveListener(Message.replay, OnReplayHandler);
    }
}