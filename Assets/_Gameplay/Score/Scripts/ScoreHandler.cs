using System;
using UnityEngine;

public class ScoreHandler : IDisposable
{
    private int streakBonus;
    private int score;
    private bool hasStreakAtLastTurn;
    private int highestScore;

    public ScoreHandler()
    {
        Messenger.AddListener<int>(Message.placeBlock, PlaceBlockHandler);
        Messenger.AddListener<int>(Message.bonus, BonusHandler);
        Messenger.AddListener(Message.newTurn, NewTurnHandler);
        Messenger.AddListener(Message.streak, StreakHandler);
        Messenger.AddListener(Message.gameOver, GameOverHandler);
        Messenger.AddListener(Message.replay, ReplayHandler);
        this.highestScore = PlayerPrefs.GetInt(PlayerPrefsVar.highestScore, 0);
        Reset();
    }

    private void ReplayHandler()
    {
        this.highestScore = PlayerPrefs.GetInt(PlayerPrefsVar.highestScore, 0);
        this.score = 0;
        this.streakBonus = 10;
        this.hasStreakAtLastTurn = false;
    }

    private void GameOverHandler()
    {
        PlayerPrefs.SetInt(PlayerPrefsVar.score, this.score);
        if (this.score > this.highestScore)
        {
            PlayerPrefs.SetInt(PlayerPrefsVar.highestScore, this.score);
        }

        PlayerPrefs.Save();
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
        Messenger.Broadcast(Message.scoreChanged, this.score);
    }

    public void Dispose()
    {
        Messenger.RemoveListener<int>(Message.placeBlock, PlaceBlockHandler);
        Messenger.RemoveListener<int>(Message.bonus, BonusHandler);
        Messenger.RemoveListener(Message.newTurn, NewTurnHandler);
        Messenger.RemoveListener(Message.streak, StreakHandler);
        Messenger.RemoveListener(Message.replay, ReplayHandler);
    }
}