using Core.AudioService;
using Core.Service;
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

    protected override void OnShow()
    {
        base.OnShow();
        this.loseView.OnClickReplay += OnClickReplayHandler;
    }

    private void OnClickReplayHandler()
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.buttonClick);
        Messenger.Broadcast(Message.replay);
    }
}