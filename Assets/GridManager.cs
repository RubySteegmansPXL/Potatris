using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject blockPrefab;
    public Block[,] grid = new Block[10, 20];
    public Vector2Int gridSize = new Vector2Int(10, 20);

    public static GridManager instance;


    public bool isResetting = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);

        }
        CreateGrid();
    }


    private void CreateGrid()
    {
        grid = new Block[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                grid[x, y] = Instantiate(blockPrefab, new Vector3(x, y, 0), Quaternion.identity).GetComponent<Block>();
                grid[x, y].SetBlockPosition(x, y);
            }
        }

        EventManager.GridCreate(new CustomEventArgs(gameObject));
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

    public Block GetBlockAt(int x, int y)
    {
        if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y) return null;
        grid[x, y].GetComponent<SpriteRenderer>().color = Color.yellow;
        return grid[x, y];
    }

    public bool IsInsideBounds(int x, int y)
    {
        return x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y;
    }

    public void AttachSegmentToBlock(ShapeSegment segment, int x, int y)
    {
        if (IsInsideBounds(x, y))
        {
            grid[x, y].AttachSegment(segment);
        }

        else
        {
            Debug.LogWarning("Tried to attach segment to block outside of bounds.");
        }
    }
}
