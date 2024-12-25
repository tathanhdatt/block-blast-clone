using System;
using System.Collections.Generic;
using Core.AudioService;
using Core.Service;
using Dt.Attribute;
using UnityEngine;

public class LevelEventHandler : MonoBehaviour, IDisposable
{
    [SerializeField, Required]
    private BoardTemplate defaultTemplate;

    [SerializeField, Required]
    private BoardGenerator boardGenerator;

    [SerializeField, Required]
    private BlockSpawner blockSpawner;

    [SerializeField, Required]
    private List<Block> blocks;

    [SerializeField, Required]
    private BlockTemplateProvider blockTemplateProvider;

    [SerializeField, ReadOnly]
    private bool isGameOver;

    [SerializeField, ReadOnly]
    private bool isRunOutOfBlock;

    [SerializeField, Required]
    private StreakHandler streakHandler;

    [SerializeField, Required]
    private ScoreHandler scoreHandler;

    private PlaceBlockHandler placeBlockHandler;
    private BoardCleaner boardCleaner;
    private GameOverChecker gameOverChecker;

    public void Initialize(BoardTemplate levelTemplate)
    {
        this.isGameOver = false;
        GenerateBoard(levelTemplate);
        InitializeTemplateProvider();
        InitializedBoardCleaner();
        InitializePlaceBlockHandler();
        InitializeScoreHandler();
        InitializeStreakHandler();
        InitializeGameOverChecker();
        SpawnBlocks(3);
        this.boardCleaner.CleanAndPlayEffect();
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.reloadLevel);
    }

    private void InitializeScoreHandler()
    {
        this.scoreHandler.Initialize();
    }


    private void InitializeTemplateProvider()
    {
        this.blockTemplateProvider.SetBoard(this.boardGenerator.Cells);
        this.blockTemplateProvider.ShuffleTemplates();
    }

    private void InitializeGameOverChecker()
    {
        this.gameOverChecker = new GameOverChecker(this.boardGenerator.Cells);
    }

    private void GenerateBoard(BoardTemplate levelTemplate)
    {
        if (levelTemplate == null)
        {
            this.boardGenerator.Initialize(this.defaultTemplate);
        }
        else
        {
            this.boardGenerator.Initialize(levelTemplate);
        }
    }

    private void InitializeStreakHandler()
    {
        this.streakHandler.Initialize();
    }

    private void SpawnBlocks(int numberOfTemplates = 1)
    {
        if (this.isGameOver) return;
        List<BlockTemplate> templates = this.blockTemplateProvider.GetTemplates(numberOfTemplates);
        Messenger.Broadcast(Message.newTurn);
        this.blocks = this.blockSpawner.Spawn(templates);
        this.placeBlockHandler.SetBlocks(this.blocks);
        CheckGameOver();
    }

    private void InitializePlaceBlockHandler()
    {
        if (this.placeBlockHandler != null)
        {
            RemovePlaceBlockHandlerEvents();
        }

        this.placeBlockHandler = new PlaceBlockHandler(this.boardGenerator.Cells);
        AddPlaceBlockHandlerEvents();
    }

    private void AddPlaceBlockHandlerEvents()
    {
        this.placeBlockHandler.OnRunOutOfBlock += OnRunOutOfBlockHandle;
        this.placeBlockHandler.ClearBoard += OnClearBoardHandler;
        this.placeBlockHandler.CheckGameOver += OnCheckGameOverHandler;
    }

    private void OnCheckGameOverHandler()
    {
        if (this.blocks.IsEmpty()) return;
        CheckGameOver();
    }

    private void CheckGameOver()
    {
        this.isGameOver = this.gameOverChecker.CheckGameOver(this.blocks);
        if (this.isGameOver)
        {
            Messenger.Broadcast(Message.gameOver);
        }
    }

    private void InitializedBoardCleaner()
    {
        this.boardCleaner = new BoardCleaner(this.boardGenerator.Cells,
            this.boardGenerator.RowEffectors,
            this.boardGenerator.ColumnEffectors,
            this.boardGenerator.BorderEffector);
    }

    private void OnClearBoardHandler(List<int> columns, List<int> rows)
    {
        this.boardCleaner.CleanAndPlayEffect(rows, columns);
        if (!this.isRunOutOfBlock) return;
        SpawnBlocks(3);
        this.isRunOutOfBlock = false;
    }

    private void OnRunOutOfBlockHandle()
    {
        this.isRunOutOfBlock = true;
    }

    public void Dispose()
    {
        RemovePlaceBlockHandlerEvents();
        this.placeBlockHandler?.Dispose();
    }

    private void RemovePlaceBlockHandlerEvents()
    {
        this.placeBlockHandler.OnRunOutOfBlock -= OnRunOutOfBlockHandle;
        this.placeBlockHandler.ClearBoard -= OnClearBoardHandler;
        this.placeBlockHandler.CheckGameOver -= OnCheckGameOverHandler;
    }

    public void Terminate()
    {
        this.boardGenerator.Terminate();
        this.blockSpawner.Terminate();
        this.blocks.Clear();
        this.isGameOver = false;
        this.isRunOutOfBlock = false;
        this.placeBlockHandler?.Dispose();
        this.boardCleaner = null;
        this.gameOverChecker = null;
        this.streakHandler?.Dispose();
        this.scoreHandler?.Dispose();
    }
}