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
        SpawnBlocks(3);
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

    private void SpawnBlocks(int numberOfTemplates = 1)
    {
        if (this.isGameOver) return;
        List<BlockTemplate> templates = new List<BlockTemplate>(3);
        for (int i = 0; i < numberOfTemplates; i++)
        {
            BlockTemplate template = this.templatePlaceableProvider.GetTemplate();
            if (template == null)
            {
                break;
            }

            templates.Add(template);
        }
        Debug.Log($"Template Count: {templates.Count}");

        if (templates.IsEmpty())
        {
            Debug.Log("No templates were spawned.");
            this.isGameOver = true;
        }
        else
        {
            this.blocks = this.blockSpawner.Spawn(templates);
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
        foreach (Block block in this.blocks)
        {
            Debug.Log(block.name, block.gameObject);
        }

        Debug.Log("Checking game over...");
        this.isGameOver = this.gameOverChecker.CheckGameOver(this.blocks);
        Debug.Log("Game over: " + this.isGameOver);
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