using System;
using System.Collections.Generic;
using CandyCoded.HapticFeedback;
using Core.AudioService;
using Core.Service;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class StreakHandler : MonoBehaviour, IDisposable
{
    [SerializeField]
    private List<GameObject> streakObjects;

    private const int MaxStreak = 10;
    private const int MinNumberOfCompletedLines = 2;
    private const int MaxNumberOfCompletedLines = 5;
    private int streakNumber;
    private bool hasStreakAtLastTurn;

    public void Initialize()
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
        ShowStreak(this.streakObjects[numberOfCompletedLines - 1]);
        Vibrate(numberOfCompletedLines);
        ServiceLocator.GetService<IAudioService>().PlaySfx(bonusSoundName);
    }

    private void Vibrate(int numberOfCompletedLines)
    {
        switch (numberOfCompletedLines)
        {
            case 2:
                HapticFeedback.LightFeedback();
                break;
            case 3:
                HapticFeedback.MediumFeedback();
                break;
            case > 3:
                HapticFeedback.HeavyFeedback();
                break;
        }
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

    private async void ShowStreak(GameObject streakObject)
    {
        streakObject.transform.localScale = Vector3.zero;
        streakObject.SetActive(true);
        Tweener tweener = streakObject.transform.DOScale(Vector3.one, 0.4f);
        tweener.SetEase(Ease.OutBack);
        await tweener.AsyncWaitForCompletion();
        await UniTask.Delay(200);
        tweener = streakObject.transform.DOScale(Vector3.zero, 0.1f);
        tweener.SetEase(Ease.OutQuad);
        await tweener.AsyncWaitForCompletion();
        streakObject.SetActive(false);
    }

    public void Dispose()
    {
        Messenger.RemoveListener(Message.newTurn, NewTurnHandler);
        Messenger.RemoveListener<int>(Message.bonus, BonusSoundHandler);
        Messenger.RemoveListener(Message.streak, IncreaseStreakHandler);
    }
}