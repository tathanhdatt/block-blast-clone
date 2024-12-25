using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour, IDisposable
{
    [SerializeField, Required]
    private TMP_Text scoreVisual;

    private int streakBonus;
    private int score;
    private bool hasStreakAtLastTurn;
    private int highestScore;

    public void Initialize()
    {
        Messenger.AddListener<int>(Message.placeBlock, PlaceBlockHandler);
        Messenger.AddListener<int>(Message.bonus, BonusHandler);
        Messenger.AddListener(Message.newTurn, NewTurnHandler);
        Messenger.AddListener(Message.streak, StreakHandler);
        Messenger.AddListener(Message.gameOver, GameOverHandler);
        Messenger.AddListener(Message.replay, ReplayHandler);
        ResetStatus();
    }

    private void ReplayHandler()
    {
        ResetStatus();
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

    public void ResetStatus()
    {
        this.highestScore = PlayerPrefs.GetInt(PlayerPrefsVar.highestScore, 0);
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
        ShowScore(score);
    }

    private async void ShowScore(int score)
    {
        this.scoreVisual.SetText($"+{score}");
        this.scoreVisual.gameObject.SetActive(true);
        this.scoreVisual.transform.localScale = Vector3.zero;
        Tweener tweener = this.scoreVisual.transform.DOScale(1, 0.2f);
        tweener.SetEase(Ease.OutBack);
        await tweener.AsyncWaitForCompletion();
        await UniTask.Delay(100);
        tweener = this.scoreVisual.transform.DOScale(0, 0.1f);
        tweener.SetEase(Ease.OutQuad);
        await tweener.AsyncWaitForCompletion();
        this.scoreVisual.gameObject.SetActive(false);
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