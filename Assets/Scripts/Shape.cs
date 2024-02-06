using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using System.Linq;

public class Shape : MonoBehaviour
{
    public List<ShapeSegment> segments;
    public ShapeSegment centerSegment;
    public Sprite bodySprite;
    public bool canRotate;

    private float timeHeld;
    private float sidewaysMoveTimer;
    private float standardMovedownTimer;
    private float SpedupDownMoveTimer;
    private bool isHoldingDown;

    private Settings settings;

    private void Start()
    {
        settings = GameManager.instance.settings;
        if (settings == null)
        {
            Debug.LogError("Settings not found", gameObject);
        }
    }

    public void MoveDown()
    {
        if (!isHoldingDown)
            Move(Vector2.down);
    }

    public void CreateSegment(int x, int y, bool isCenter, SpriteData data, Sprite[] sprites, Sprite[] faces)
    {
        if (!IsValidPosition(x, y))
        {
            Debug.LogWarning("Invalid position, so not creating segment.");
            return;
        }
        ShapeSegment newSegment = new GameObject().AddComponent<ShapeSegment>();
        newSegment.transform.parent = transform;
        newSegment.Instantiate(data, sprites, faces);
        newSegment.Create(x, y);
        newSegment.tag = "TetrisBlock";
        // Transparent 50  

        segments.Add(newSegment);

        if (isCenter)
        {
            centerSegment = newSegment;
        }
    }

    public bool IsValidPosition(int x, int y)
    {
        // Get the block at the position
        Block block = GridManager.instance.GetBlockAt(x, y);

        // If the block is null, something went wrong, or we went outside the bounds, so return false
        if (block == null)
        {
            Debug.LogWarning("Block is null, so not a valid position. " + x + ", " + y + " is outside the bounds.");
            return false;
        }

        // If the block is occupied by a segment not from this shape, return false
        if (block.isOccupied && !segments.Contains(block.segment))
        {
            return false;
        }

        return true;
    }

    private void Update()
    {

        standardMovedownTimer += Time.deltaTime;
        if (standardMovedownTimer > settings.defaultMoveSpeed)
        {
            MoveDown();
            standardMovedownTimer = 0;
        }
        if (GameManager.instance.gameState == GameState.TUTORIAL_TOTALBLOCK)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2.right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2.left);
        }

        // Check for holding down left and right arrow keys
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            timeHeld += Time.deltaTime;
            if (timeHeld > settings.timeToHoldBeforeSpeedup)
            {
                sidewaysMoveTimer += Time.deltaTime;
                if (sidewaysMoveTimer > settings.holdMoveSpeed)
                {
                    Move(Input.GetKey(KeyCode.LeftArrow) ? Vector2.left : Vector2.right);
                    sidewaysMoveTimer = 0;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            sidewaysMoveTimer = 0;
            timeHeld = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SpedupDownMoveTimer = settings.holdMoveSpeed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            isHoldingDown = true;
            SpedupDownMoveTimer += Time.deltaTime;
            if (SpedupDownMoveTimer > settings.holdMoveSpeed)
            {
                Move(Vector2.down);
                SpedupDownMoveTimer = 0;
                // Make sure there is some delay before the next move down
                standardMovedownTimer = settings.defaultMoveSpeed / 2;
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isHoldingDown = false;
            // Make sure there is some delay before the next move down
            standardMovedownTimer = settings.defaultMoveSpeed / 2;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateShape();
        }
    }

    public void RotateShape()
    {
        if (!canRotate || settings == null || GameManager.instance.gameState == GameState.PAUSE || GameManager.instance.gameState == GameState.TUTORIAL || GameManager.instance.gameState == GameState.TUTORIAL_MOVEBLOCK || GameManager.instance.gameState == GameState.TUTORIAL_TOTALBLOCK)
        {
            return;
        }

        bool rotationApplied = false;
        int nudgeDirection = -1; // Start with nudging to the left
        int nudgeAmount = 0;
        Vector2[] newPositions = new Vector2[segments.Count];

        while (!rotationApplied && nudgeAmount <= settings.maximumNudgeTries)
        {
            bool validRotation = true;

            // Get the rotation coordinates of all the segments
            for (int i = 0; i < segments.Count; i++)
            {
                newPositions[i] = RotateAroundCenter(segments[i].position, true) + new Vector2(nudgeAmount * nudgeDirection, 0);
                if (!IsValidPosition((int)newPositions[i].x, (int)newPositions[i].y))
                {
                    validRotation = false;
                    break; // Exit if any position is invalid
                }
            }

            if (validRotation && TrajectoryClear(centerSegment.position, newPositions[segments.IndexOf(centerSegment)]))
            {
                rotationApplied = true;
            }
            else
            {
                // Switch between nudging to the left and right
                if (nudgeDirection == -1)
                {
                    nudgeDirection = 1; // Switch to nudging to the right
                }
                else
                {
                    nudgeDirection = -1; // Switch back to nudging to the left
                    nudgeAmount++; // Increase nudge amount after trying both directions
                }
            }
        }

        if (!rotationApplied)
        {
            Debug.LogWarning("Rotation not applied after maximum nudge attempts.");
            return;
        }

        // Detach, set new positions, and re-attach
        DetachAllSegments();
        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].position = newPositions[i];
        }
        AttachAllSegments();
        EventManager.BlockRotate(new CustomEventArgs(gameObject));
    }

    private bool TrajectoryClear(Vector2 originalPosition, Vector2 newPosition)
    {
        Vector2 direction = newPosition - originalPosition;
        int steps = Mathf.Max(Mathf.Abs((int)direction.x), Mathf.Abs((int)direction.y));
        Vector2 step = direction / steps;

        for (int i = 1; i <= steps; i++)
        {
            Vector2 currentPosition = originalPosition + step * i;
            if (!IsValidPosition((int)currentPosition.x, (int)currentPosition.y))
            {
                return false; // An intermediate position is occupied
            }
        }
        return true; // All intermediate positions are unoccupied
    }


    private Vector2 RotateAroundCenter(Vector2 position, bool clockWise)
    {
        if (centerSegment == null)
        {
            Debug.LogWarning("Center segment is null, so not rotating.");
            return Vector2.zero;
        }

        Vector2 center = centerSegment.position;
        Vector2 direction = position - center;

        if (clockWise)
        {
            direction = new Vector2(direction.y, -direction.x);
        }
        else
        {
            direction = new Vector2(-direction.y, direction.x);
        }

        Vector2 newPosition = center + direction;
        return newPosition; // Return new position regardless of validity
    }



    public bool CanMove(Vector2 direction)
    {
        foreach (ShapeSegment segment in segments)
        {
            Vector2 newPosition = segment.position + direction;
            if (!IsValidPosition((int)newPosition.x, (int)newPosition.y))
            {
                return false; // Early exit if any new position is invalid
            }
        }
        return true; // All new positions are valid
    }

    public void Move(Vector2 direction)
    {
        if (GameManager.instance.gameState == GameState.PAUSE || GameManager.instance.gameState == GameState.TUTORIAL)
        {
            Debug.LogWarning("Game is paused, so not moving.");
            return;
        }

        // ignore down movement during the user block tutorial
        if (GameManager.instance.gameState == GameState.TUTORIAL_MOVEBLOCK && direction != Vector2.down)
        {
            return;
        }

        // Ignore all movement during the total block tutorial except for natural down movement
        if (GameManager.instance.gameState == GameState.TUTORIAL_TOTALBLOCK && (isHoldingDown || direction != Vector2.down))
        {
            return;
        }


        if (!CanMove(direction))
        {
            // This means we've reached the bottom
            if (direction == Vector2.down)
            {
                Dissolve();
            }



            Debug.LogWarning("Movement is not possible: " + direction);
            return; // Early exit if movement is paused or not possible
        }

        DetachAllSegments();
        foreach (ShapeSegment segment in segments)
        {
            segment.position = new Vector2(segment.position.x + direction.x, segment.position.y + direction.y);
        }

        if (direction == Vector2.down)
        {
            EventManager.MovementDown(new CustomEventArgs(gameObject));
        }
        else if (direction == Vector2.left || direction == Vector2.right)
        {
            EventManager.Movement(new CustomEventArgs(gameObject));
        }

        AttachAllSegments();
    }

    public void Dissolve()
    {
        foreach (ShapeSegment segment in segments)
        {
            segment.transform.parent = null;
        }

        ShapeFactory.instance.DissolveShape();

    }

    public void DetachAllSegments()
    {
        foreach (ShapeSegment segment in segments)
        {
            GridManager.instance.DetachSegmentFromBlock((int)segment.position.x, (int)segment.position.y);
        }
    }

    public void AttachAllSegments()
    {
        foreach (ShapeSegment segment in segments)
        {
            GridManager.instance.AttachSegmentToBlock(segment, (int)segment.position.x, (int)segment.position.y);
        }
    }
}
