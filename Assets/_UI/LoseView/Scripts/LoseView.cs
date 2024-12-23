using System;
using Cysharp.Threading.Tasks;
using Dt.Attribute;
using UnityEngine;
using UnityEngine.UI;

public class LoseView : BaseView
{
    [SerializeField, Required]
    private Button replayButton;

    public event Action OnClickReplay;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        this.replayButton.onClick.AddListener(() => OnClickReplay?.Invoke());
    }
}