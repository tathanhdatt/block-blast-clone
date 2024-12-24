using System;
using Core.AudioService;
using Core.Service;
using UnityEngine;

public class ScoreHandler : IDisposable
{
    private int streakBonus = 0;
    private int score = 0;
    private bool hasStreakAtLastTurn = false;
    private int highestScore;

    public ScoreHandler()
    {
        Messenger.AddListener<int>(Message.placeBlock, PlaceBlockHandler);
        Messenger.AddListener<int>(Message.bonus, BonusHandler);
        Messenger.AddListener(Message.newTurn, NewTurnHandler);
        Messenger.AddListener(Message.streak, StreakHandler);
        Messenger.AddListener(Message.gameOver, GameOverHandler);
        this.highestScore = PlayerPrefs.GetInt("highest_score", 0);
        Reset();
    }

    private void GameOverHandler()
    {
        if (this.score > this.highestScore)
        {
            PlayerPrefs.SetInt(PlayerPrefsVar.highestScore, this.score);
        }
    }

    private void StreakHandler()
    {
        this.streakBonus += 10;
        this.hasStreakAtLastTurn = true;
    }

    private void NewTurnHandler()
    {
        if (!this.hasStreakAtLastTurn)
        {
            this.streakBonus = 10;
        }

        this.hasStreakAtLastTurn = false;
    }

    private void BonusHandler(int multiplier)
    {
        AddScore(multiplier * this.streakBonus);
    }

    public void Reset()
    {
        this.score = 0;
        this.streakBonus = 10;
        this.hasStreakAtLastTurn = false;
    }

    private void PlaceBlockHandler(int score)
    {
        AddScore(score);
    }

    private void AddScore(int score)
    {
        this.score += score;
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.scoreUp);
        Messenger.Broadcast(Message.scoreChanged, this.score);
    }

    public void Dispose()
    {
        Messenger.RemoveListener<int>(Message.placeBlock, PlaceBlockHandler);
        Messenger.RemoveListener<int>(Message.bonus, BonusHandler);
        Messenger.RemoveListener(Message.newTurn, NewTurnHandler);
        Messenger.RemoveListener(Message.streak, StreakHandler);
    }
}