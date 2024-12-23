﻿using System;

public class ScoreHandler : IDisposable
{
    private int streakBonus = 0;
    private int score = 0;
    private bool hasStreakAtLastTurn = false;

    public ScoreHandler()
    {
        Messenger.AddListener<int>(Message.placeBlock, PlaceBlockHandler);
        Messenger.AddListener<int>(Message.bonus, BonusHandler);
        Messenger.AddListener(Message.newTurn, NewTurnHandler);
        Messenger.AddListener(Message.streak, StreakHandler);
        Reset();
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
    }
}