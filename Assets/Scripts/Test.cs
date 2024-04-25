using Board.BoardCreation;
using Cysharp.Threading.Tasks;
using Level;
using Services.PoolingService;
using UnityEngine;
using VContainer;

public class Test : MonoBehaviour
{
    [Inject] private IPoolService _poolService;
    [Inject] private BoardCreationData _creationData;
    private BlockCreator _blockCreator;
    
    private const int GridSizeX = 6;
    private const int GridSizeY = 6;
    public GameObject cellPrefab; 

    public Vector2 centerPosition;

    private async void Start()
    {
        _blockCreator = new BlockCreator(_poolService, _creationData);
        
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        var startPosition = new Vector3(centerPosition.x - (GridSizeX - 1) / 2f,
            centerPosition.y - (GridSizeY - 1) / 2f, 0);

        for (var row = 0; row < GridSizeX; row++)
        for (var col = 0; col < GridSizeY; col++)
        {
            var cellPosition = new Vector3(startPosition.x + row, startPosition.y + col, 0);

            // Instantiate(cellPrefab, cellPosition, Quaternion.identity, transform);
            var block = _blockCreator.GetBlock((BlockType)Random.Range(0, 4));
            block.transform.position = cellPosition;
        }
    }
}