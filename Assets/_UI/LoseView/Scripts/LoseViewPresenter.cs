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
        Messenger.Broadcast(Message.replay);
    }
}