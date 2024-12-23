using System;
using System.Globalization;
using DG.Tweening;
using Dt.Attribute;
using TMPro;
using UnityEngine;

public class GameView : BaseView
{
    [SerializeField, Required]
    private TMP_Text scoreText;
    
    private Tweener scoreTweener;

    public void UpdateScore(int score)
    {
        this.scoreTweener.Kill();
        int lastScore = int.Parse(this.scoreText.text);
        this.scoreTweener = DOTween.To(val =>
        {
            int tempScore = Mathf.FloorToInt(val);
            this.scoreText.SetText(tempScore.ToString());
        }, lastScore, score, 0.2f);
        this.scoreTweener.SetEase(Ease.OutQuad);
    }
}