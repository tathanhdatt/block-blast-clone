using Core.AudioService;
using Core.Service;
using Dt.Attribute;
using UnityEngine;

namespace Core.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Required]
        private GamePresenter presenter;

        [SerializeField, Required]
        private LevelPlayer levelPlayer;

        private BoardTemplateDatabase boardTemplateDatabase;
        private IAudioService audioService;

        private async void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Application.targetFrameRate = 60;
            InitializeBoardDatabase();
            InitAudioService();
            Messenger.AddListener(Message.gameOver, GameOverHandler);
        }

        private void GameOverHandler()
        {
            ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.noSpace);
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

        private void Start()
        {
            this.presenter.Enter(this);
            this.presenter.InitialViewPresenters();
            OnEnter();
        }
        
        private async void OnEnter()
        {
            PlayLevel(0);
        }

        private void PlayLevel(int level)
        {
            BoardTemplate levelTemplate = this.boardTemplateDatabase.GetTemplate(level);
            this.levelPlayer.Play(levelTemplate);
        }

        private void LevelWinHandler()
        {
        }
    }
}