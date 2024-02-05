using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject blockPrefab;
    public Block[,] grid;
    public Vector2Int gridSize = new Vector2Int(11, 20);

    public static GridManager instance;

    private Settings settings;


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

        settings = GameManager.instance.settings;
        if (settings == null)
        {
            Debug.LogError("Settings not found", gameObject);
        }

        Debug.Log(settings);

        gridSize = new Vector2Int(settings.numberOfColumns, settings.numberOfRows);
        grid = new Block[gridSize.x, gridSize.y];
        CreateGrid();
    }

    private void Update()
    {
        //debugging
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ResetGrid();
        }
    }


    public void ResetGrid()
    {
        isResetting = true;
        foreach (Block block in grid)
        {
            block.Reset();
        }

        ShapeFactory.instance.Reset();

        isResetting = false;
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
        if (settings == null) return;
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

    public void DetachSegmentFromBlock(int x, int y)
    {
        if (IsInsideBounds(x, y))
        {
            grid[x, y].DetachSegment();
        }
        else
        {
            Debug.LogWarning("Tried to detach segment from block outside of bounds.");
        }
    }

    public void CheckForLine()
    {
        StartCoroutine(ICheckForLine());
    }

    public IEnumerator ICheckForLine()
    {
        int linesCleared = 0;
        GameManager.instance.PauseGame();
        for (int y = 0; y < gridSize.y; y++)
        {
            if (IsLineFull(y))
            {
                Debug.Log("Line cleared");
                ClearLine(y);
                MoveDown(y);
                EventManager.FullRow(new CustomEventArgs(gameObject), 1);
                yield return new WaitForSeconds(settings.lineClearDelay);
                y--; // Check the same line again
                linesCleared++;
            }
        }
        GameManager.instance.ResumeGame();
        Debug.LogWarning("Lines cleared: " + linesCleared);
    }

    private bool IsLineFull(int y)
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            if (!grid[x, y].isOccupied)
            {
                return false;
            }
        }
        return true;
    }

    private void ClearLine(int y)
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            grid[x, y].Reset();
        }
    }

    private void MoveDown(int y)
    {
        for (int i = y; i < gridSize.y - 1; i++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {


                if (grid[x, i + 1].isOccupied)
                {
                    grid[x, i].AttachSegment(grid[x, i + 1].segment);
                    grid[x, i + 1].DetachSegment();
                    // Update segment position
                    grid[x, i].segment.position = new Vector2Int(x, i);
                }

            }
        }
    }
}
