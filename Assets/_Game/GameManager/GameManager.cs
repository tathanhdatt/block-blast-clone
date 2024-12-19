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

    private void Awake()
    {
        Application.targetFrameRate = 60;
        GenerateBoard();
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
    }

    private void OnRunOutOfBlockHandle()
    {
        SpawnBlocks();
    }
}