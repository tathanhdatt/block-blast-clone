using Core.AudioService;
using Core.Service;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoseViewPresenter : BaseViewPresenter
{
    private LoseView loseView;

    public LoseViewPresenter(GamePresenter gamePresenter, Transform transform) : base(gamePresenter,
        transform)
    {
    }

    protected override void AddViews()
    {
        this.loseView = AddView<LoseView>();
    }

    protected override async void OnShow()
    {
        base.OnShow();
        this.loseView.OnClickReplay += OnClickReplayHandler;
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.showLoseView);
        await ShowScore();
        await UniTask.Delay(500);
        await this.loseView.ScaleUpButton();
    }

    private async UniTask ShowScore()
    {
        int lastScore = PlayerPrefs.GetInt(PlayerPrefsVar.score, 0);
        int highestScore = PlayerPrefs.GetInt(PlayerPrefsVar.highestScore, 0);
        await this.loseView.ShowScore(lastScore);
        await UniTask.Delay(200);
        if (lastScore >= highestScore)
        {
            ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.newHighScore);
            this.loseView.PlayConfetti();
            this.loseView.EnableCrown();
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        this.loseView.OnClickReplay -= OnClickReplayHandler;
        this.loseView.DisableCrown();
    }

    private void OnClickReplayHandler()
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.buttonClick);
        Messenger.Broadcast(Message.replay);
    }
}