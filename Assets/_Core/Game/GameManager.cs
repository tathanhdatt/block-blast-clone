using System;
using Core.AudioService;
using Core.Service;
using Cysharp.Threading.Tasks;
using Dt.Attribute;
using UnityEngine;

namespace Core.Game
{
    public class GameManager : MonoBehaviour, IDisposable
    {
        [SerializeField, Required]
        private GamePresenter presenter;

        [SerializeField, Required]
        private LevelPlayer levelPlayer;

        private BoardTemplateDatabase boardTemplateDatabase;
        private IAudioService audioService;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Application.targetFrameRate = 60;
            InitializeBoardDatabase();
            InitAudioService();
            Messenger.AddListener(Message.gameOver, GameOverHandler);
            Messenger.AddListener(Message.replay, ReplayHandler);
        }

        private void ReplayHandler()
        {
            this.presenter.GetViewPresenter<LoseViewPresenter>().Hide();
            this.levelPlayer.Terminate();
            PlayLevel(0);
        }

        private async void GameOverHandler()
        {
            await UniTask.Delay(600);
            ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.noSpace);
            await this.presenter.GetViewPresenter<LoseViewPresenter>().Show();
        }

        private void InitializeBoardDatabase()
        {
            this.boardTemplateDatabase = new BoardTemplateDatabase();
        }

        private void InitAudioService()
        {
            this.audioService = FindAnyObjectByType<NativeAudioService>();
            ServiceLocator.Register(this.audioService);
        }

        private async void Start()
        {
            this.presenter.Enter(this);
            await this.presenter.InitialViewPresenters();
            await OnEnter();
        }

        private async UniTask OnEnter()
        {
            await this.presenter.GetViewPresenter<LoadingViewPresenter>().Show();
            await this.presenter.GetViewPresenter<LoadingViewPresenter>().Hide();
            await this.presenter.GetViewPresenter<GameViewPresenter>().Show();
            PlayLevel(0);
        }

        private void PlayLevel(int level)
        {
            BoardTemplate levelTemplate = this.boardTemplateDatabase.GetTemplate(level);
            this.levelPlayer.Play(levelTemplate);
        }

        public void Dispose()
        {
            Messenger.RemoveListener(Message.gameOver, GameOverHandler);
            Messenger.RemoveListener(Message.replay, ReplayHandler);
        }
    }
}