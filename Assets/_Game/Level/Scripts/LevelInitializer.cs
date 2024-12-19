using System;
using System.Collections.Generic;
using Dt.Attribute;
using UnityEngine;

public class LevelInitializer : MonoBehaviour, IDisposable
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
    private TemplatePlaceableProvider templatePlaceableProvider;

    [SerializeField, ReadOnly]
    private bool isGameOver;

    private PlaceBlockHandler placeBlockHandler;
    private BoardCleaner boardCleaner;
    private GameOverChecker gameOverChecker;

    public void Initialize(BoardTemplate levelTemplate)
    {
        Application.targetFrameRate = 60;
        this.isGameOver = false;
        GenerateBoard(levelTemplate);
        InitializeTemplateProvider();
        InitializedBoardCleaner();
        InitializePlaceBlockHandler();
        SpawnBlocks();
        InitializeGameOverChecker();
        this.boardCleaner.Clean();
    }

    private void InitializeTemplateProvider()
    {
        this.templatePlaceableProvider.SetBoardCells(this.boardGenerator.Cells);
        this.templatePlaceableProvider.ShuffleTemplates();
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

    private void SpawnBlocks()
    {
        if (this.isGameOver) return;
        BlockTemplate template = this.templatePlaceableProvider.GetTemplate();
        if (template == null)
        {
            this.isGameOver = true;
        }
        else
        {
            this.blocks = this.blockSpawner.Spawn(template);
            this.placeBlockHandler.SetBlocks(this.blocks);
        }
    }

    private void InitializePlaceBlockHandler()
    {
        this.placeBlockHandler = new PlaceBlockHandler();
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
    }

    private void InitializedBoardCleaner()
    {
        this.boardCleaner = new BoardCleaner(this.boardGenerator.Cells);
    }

    private void OnClearBoardHandler(List<int> x, List<int> y)
    {
        this.boardCleaner.Clean(y, x);
    }

    private void OnRunOutOfBlockHandle()
    {
        SpawnBlocks();
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