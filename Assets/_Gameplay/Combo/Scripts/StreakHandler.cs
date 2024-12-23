using System;
using Core.AudioService;
using Core.Service;
using UnityEngine;

public class StreakHandler : IDisposable
{
    private const int MaxStreak = 10;
    private const int MinNumberOfCompletedLines = 2;
    private const int MaxNumberOfCompletedLines = 5;
    private int streakNumber;
    private bool hasStreakAtLastTurn;

    public StreakHandler()
    {
        Messenger.AddListener(Message.newTurn, NewTurnHandler);
        Messenger.AddListener<int>(Message.bonus, BonusSoundHandler);
        Messenger.AddListener(Message.streak, IncreaseStreakHandler);
        this.streakNumber = 0;
        this.hasStreakAtLastTurn = false;
    }

    private void IncreaseStreakHandler()
    {
        this.hasStreakAtLastTurn = true;
        this.streakNumber = Math.Clamp(this.streakNumber + 1, 0, MaxStreak);
        HandleStreak();
    }

    private void BonusSoundHandler(int numberOfCompletedLines)
    {
        if (numberOfCompletedLines < MinNumberOfCompletedLines) return;
        numberOfCompletedLines = Math.Clamp(numberOfCompletedLines,
            MinNumberOfCompletedLines, MaxNumberOfCompletedLines);
        string bonusSoundName = $"lines_{numberOfCompletedLines}";
        Debug.Log(bonusSoundName);
        ServiceLocator.GetService<IAudioService>().PlaySfx(bonusSoundName);
    }

    private void NewTurnHandler()
    {
        if (!this.hasStreakAtLastTurn)
        {
            this.streakNumber = 0;
        }

        this.hasStreakAtLastTurn = false;
    }

    private void HandleStreak()
    {
        string streakSoundName = $"streak_{this.streakNumber}";
        ServiceLocator.GetService<IAudioService>().PlaySfx(streakSoundName);
    }

    public void Dispose()
    {
        Messenger.RemoveListener(Message.newTurn, NewTurnHandler);
        Messenger.RemoveListener<int>(Message.bonus, BonusSoundHandler);
        Messenger.RemoveListener(Message.streak, IncreaseStreakHandler);
    }
}