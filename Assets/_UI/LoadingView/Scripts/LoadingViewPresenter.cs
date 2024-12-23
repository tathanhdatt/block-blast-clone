using UnityEngine;

public class LoadingViewPresenter : BaseViewPresenter
{
    private LoadingView loadingView;

    public LoadingViewPresenter(GamePresenter gamePresenter, Transform transform) : base(
        gamePresenter, transform)
    {
    }

    protected override void AddViews()
    {
        this.loadingView = AddView<LoadingView>();
    }
}