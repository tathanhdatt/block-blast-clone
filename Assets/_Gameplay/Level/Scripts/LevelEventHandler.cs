using System;
using System.Collections.Generic;
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

    private PlaceBlockHandler placeBlockHandler;
    private BoardCleaner boardCleaner;
    private GameOverChecker gameOverChecker;
    private StreakHandler streakHandler;

    public void Initialize(BoardTemplate levelTemplate)
    {
        this.isGameOver = false;
        GenerateBoard(levelTemplate);
        InitializeTemplateProvider();
        InitializedBoardCleaner();
        InitializePlaceBlockHandler();
        InitializeStreakHandler();
        SpawnBlocks(3);
        InitializeGameOverChecker();
        this.boardCleaner.CleanAndPlayEffect();
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
        this.streakHandler = new StreakHandler();
    }

    private void SpawnBlocks(int numberOfTemplates = 1)
    {
        if (this.isGameOver) return;
        List<BlockTemplate> templates =
            this.blockTemplateProvider
                .GetPlaceableBlockTemplates(numberOfTemplates);
        int numberOfRandomBlocks = GameConstant.maxBlocks - templates.Count;
        if (numberOfRandomBlocks > 0)
        {
            templates.AddRange(this.blockTemplateProvider
                .GetRandomBlockTemplates(numberOfRandomBlocks));
        }

        if (templates.IsEmpty())
        {
            Messenger.Broadcast(Message.gameOver);
            this.isGameOver = true;
        }
        else
        {
            Messenger.Broadcast(Message.newTurn);
            this.blocks = this.blockSpawner.Spawn(templates);
            this.placeBlockHandler.SetBlocks(this.blocks);
        }
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
        SpawnBlocks(2);
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
}