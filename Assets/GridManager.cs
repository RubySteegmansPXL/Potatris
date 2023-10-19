using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject blockPrefab;
    public Block[,] grid = new Block[10, 20];

    public Vector2Int gridSize = new Vector2Int(10, 20);

    private void Start()
    {
        grid = new Block[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[x, y] = Instantiate(blockPrefab, new Vector3(x, y, 0), Quaternion.identity).GetComponent<Block>();
                grid[x, y].SetPosition(x, y);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        if (grid == null || grid[0, 0] == null) return;
        foreach (Block block in grid)
        {
            Gizmos.DrawWireCube(block.transform.position, Vector3.one);
        }
    }
}
