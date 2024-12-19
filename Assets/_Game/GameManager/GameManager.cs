using System.Collections.Generic;
using Dt.Attribute;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Required]
    private BoardGenerator boardGenerator;

    [SerializeField, Required]
    private BlockSpawner blockSpawner;

    [SerializeField, Required]
    private List<Block> blocks;

    private PlaceBlockHandler placeBlockHandler;
    private BoardCleaner boardCleaner;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        GenerateBoard();
        InitializedBoardCleaner();
        InitializePlaceBlockHandler();
        SpawnBlocks();
    }

    private void GenerateBoard()
    {
        this.boardGenerator.Initialize();
    }

    private void SpawnBlocks()
    {
        this.blocks = this.blockSpawner.Spawn();
        this.placeBlockHandler.SetBlocks(this.blocks);
    }

    private void InitializePlaceBlockHandler()
    {
        this.placeBlockHandler = new PlaceBlockHandler();
        this.placeBlockHandler.OnRunOutOfBlock += OnRunOutOfBlockHandle;
        this.placeBlockHandler.ClearBoard += OnClearBoardHandler;
    }

    private void InitializedBoardCleaner()
    {
        this.boardCleaner = new BoardCleaner(this.boardGenerator.Cells);
    }

    private void OnClearBoardHandler(List<int> x, List<int> y)
    {
        foreach (int column in x)
        {
            this.boardCleaner.CleanColumnIfFull(column);
        }

        foreach (int row in y)
        {
            this.boardCleaner.CleanRowIfFull(row);
        }
    }

    private void OnRunOutOfBlockHandle()
    {
        SpawnBlocks();
    }
}