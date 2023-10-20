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
                grid[x, y].SetPosition(x, y);
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

    public bool CheckForLowerCollision(int x, int y)
    {
        if (y <= 0) return true;
        return grid[x, y - 1].isOccupied;
    }

    public bool CheckForLeftCollision(int x, int y)
    {
        if (x <= 0) return true;
        return grid[x - 1, y].isOccupied;
    }

    public bool CheckForRightCollision(int x, int y)
    {
        if (x >= gridSize.x - 1) return true;
        return grid[x + 1, y].isOccupied;
    }

    public Block GetBlockBelow(int x, int y)
    {
        if (y <= 0) return null;
        return grid[x, y - 1];
    }

    public void MoveBlock(ShapeSegment segment, int originalX, int originalY, int newX, int newY)
    {
        if (!IsInsideBounds(newX, newY))
        {
            Debug.LogError("Block is outside of bounds");
            return;
        }

        Block originalBlock = null;

        if (IsInsideBounds(originalX, originalY))
        {
            originalBlock = grid[originalX, originalY];
        }
        Block newBlock = grid[newX, newY];

        if (newBlock == null)
        {
            Debug.LogError("Block is null");
            return;
        }

        if (originalBlock == newBlock) return;

        if (originalBlock != null && originalBlock.segment == segment)
            originalBlock.SetUnoccupied();

        newBlock.SetOccupied(segment);
    }

    public void CreateBlock(ShapeSegment segment, int x, int y)
    {
        grid[x, y].SetOccupied(segment);
    }

    public Block GetBlockRight(int x, int y)
    {
        x += 1;
        if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y) return null;
        return grid[x, y];
    }

    public Block GetBlockLeft(int x, int y)
    {
        x -= 1;
        if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y) return null;
        return grid[x, y];
    }

    public Block GetBlockAt(int x, int y)
    {
        if (x < 0 || x >= gridSize.x || y < 0 || y >= gridSize.y) return null;
        grid[x, y].GetComponent<SpriteRenderer>().color = Color.yellow;
        return grid[x, y];
    }

    public List<int> CheckForLines()
    {
        List<int> lines = new List<int>();
        for (int y = 0; y < gridSize.y; y++)
        {
            bool line = true;
            for (int x = 0; x < gridSize.x; x++)
            {
                if (!grid[x, y].isOccupied)
                {
                    line = false;
                    break;
                }
            }

            if (line)
            {
                lines.Add(y);
            }
        }

        foreach (int line in lines)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                grid[x, line].SetLine();
            }

            // Move all blocks above the line down
            for (int y = line + 1; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    grid[x, y].MoveDownSegment();
                }
            }
        }

        return lines;
    }

    public bool IsInsideBounds(int x, int y)
    {
        return x >= 0 && x < gridSize.x && y >= 0 && y < gridSize.y;
    }

    public int SideOfBoard(int x)
    {
        if (x < gridSize.x / 2)
            return -1;
        else if (x > gridSize.x / 2)
            return 1;
        else
            return 0;
    }

}
