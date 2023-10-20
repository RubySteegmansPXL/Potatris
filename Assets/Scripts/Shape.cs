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
    public float defaultMoveSpeed = 3f;
    public bool canRotate;

    public bool pauseMovement;

    public bool isMoving = false;



    public void CreateSegment(int x, int y, bool isCenter, SpriteData data, Sprite[] sprites, Sprite[] faces)
    {
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

    public void SetPosition(int x, int y)
    {
        transform.position = new Vector3(0, 0, 0);
        // Set the position of the shape according to the center segment;
        int xOffset = x - centerSegment.x;
        int yOffset = y - centerSegment.y;

        foreach (ShapeSegment segment in segments)
        {
            segment.Move(segment.x + xOffset, segment.y + yOffset);
        }
    }

    private void Update()
    {
        MoveDownSmoothly();

        if (Input.GetKey(KeyCode.DownArrow))
        {
            defaultMoveSpeed = 8f;
        }
        else
        {
            defaultMoveSpeed = 3f;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && canRotate)
        {
            RotateShapeRight();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (pauseMovement)
        {
            defaultMoveSpeed = 0f;
        }
    }

    public void MoveDownSmoothly()
    {
        if (isMoving) return;

        // If any of the blocks in the shape are not moveable, don't move the shape
        foreach (ShapeSegment segment in segments)
        {
            if (!segment.canMove) return;
            segment.MoveDown();
        }

        StartCoroutine(MoveDownSmoothlyCoroutine());
    }

    private IEnumerator MoveDownSmoothlyCoroutine()
    {
        transform.position = new Vector3(0, 1, 0);
        isMoving = true;
        float accumulatedMove = 0f;
        Vector3 startPosition = transform.position;

        while (accumulatedMove < 1f)
        {
            float distanceToMoveThisFrame = defaultMoveSpeed * Time.deltaTime;
            accumulatedMove += distanceToMoveThisFrame;

            // Avoid overshooting the target
            if (accumulatedMove > 1f)
            {
                distanceToMoveThisFrame -= accumulatedMove - 1f;
            }

            transform.position -= new Vector3(0, distanceToMoveThisFrame, 0);
            yield return null;
        }

        // Make sure to end up exactly 1 unit lower to avoid floating point errors
        transform.position = new Vector3(startPosition.x, startPosition.y - 1, startPosition.z);
        isMoving = false;

        // Then, reset own position and change positions of all segments in the shape

        transform.position = new Vector3(0, 0, 0);

        CheckBottomCollision();
    }

    public void RotateShapeRight()
    {
        // Store the original positions in case we need to revert the rotation
        List<Vector2Int> originalPositions = new List<Vector2Int>();
        foreach (ShapeSegment segment in segments)
        {
            originalPositions.Add(new Vector2Int(segment.x, segment.y));
        }

        List<Vector2Int> newPositions = new List<Vector2Int>();

        // Calculate the hypothetical positions after rotation
        foreach (ShapeSegment segment in segments)
        {
            if (segment != centerSegment)  // No need to rotate the center segment
            {
                int relativeX = segment.x - centerSegment.x;
                int relativeY = segment.y - centerSegment.y;

                int rotatedX = relativeY;
                int rotatedY = -relativeX;

                int newX = rotatedX + centerSegment.x;
                int newY = rotatedY + centerSegment.y;

                newPositions.Add(new Vector2Int(newX, newY));
            }
            else
            {
                // Center segment remains the same
                newPositions.Add(new Vector2Int(segment.x, segment.y));
            }
        }

        // Check for collisions at the new positions
        bool collisionFound = false;
        foreach (Vector2Int pos in newPositions)
        {
            if (!IsValidPosition(pos.x, pos.y))
            {
                collisionFound = true;
                break;
            }
        }

        if (!collisionFound)
        {
            // If no collisions, move to the new positions
            for (int i = 0; i < segments.Count; i++)
            {
                segments[i].Move(newPositions[i].x, newPositions[i].y);
            }
        }

        else
        {
            // Handle wall kicks: left, right, up, etc.
            // For simplicity, let's just try a move to the right and then to the left
            List<Vector2Int> shiftedRight = newPositions.Select(pos => new Vector2Int(pos.x + 1, pos.y)).ToList();
            List<Vector2Int> shiftedLeft = newPositions.Select(pos => new Vector2Int(pos.x - 1, pos.y)).ToList();

            bool canShiftRight = false;
            int maxHorizontalShift = segments.Max(s => Mathf.Abs(s.x - centerSegment.x));  // roughly the width of the shape

            // Try shifting right
            for (int shift = 1; shift <= maxHorizontalShift; shift++)
            {
                List<Vector2Int> shiftedPositions = newPositions.Select(pos => new Vector2Int(pos.x + shift, pos.y)).ToList();
                if (!shiftedPositions.Any(pos => !IsValidPosition(pos.x, pos.y)))
                {
                    canShiftRight = true;
                    for (int i = 0; i < segments.Count; i++)
                    {
                        segments[i].Move(shiftedPositions[i].x, shiftedPositions[i].y);
                    }
                    break;
                }
            }

            // If couldn't shift right, try shifting left
            if (!canShiftRight)
            {
                for (int shift = 1; shift <= maxHorizontalShift; shift++)
                {
                    List<Vector2Int> shiftedPositions = newPositions.Select(pos => new Vector2Int(pos.x - shift, pos.y)).ToList();
                    if (!shiftedPositions.Any(pos => !IsValidPosition(pos.x, pos.y)))
                    {
                        for (int i = 0; i < segments.Count; i++)
                        {
                            segments[i].Move(shiftedPositions[i].x, shiftedPositions[i].y);
                        }
                        break;
                    }
                }
            }


            // If still colliding after horizontal wall kicks, try vertical wall kicks (move upwards)
            int maxVerticalShift = segments.Max(s => Mathf.Abs(s.y - centerSegment.y));  // roughly the height of the shape

            for (int verticalShift = 1; verticalShift <= maxVerticalShift; verticalShift++)
            {
                List<Vector2Int> shiftedUp = newPositions.Select(pos => new Vector2Int(pos.x, pos.y + verticalShift)).ToList();
                if (!shiftedUp.Any(pos => !IsValidPosition(pos.x, pos.y)))
                {
                    for (int i = 0; i < segments.Count; i++)
                    {
                        segments[i].Move(shiftedUp[i].x, shiftedUp[i].y);
                    }
                    break;
                }
            }
        }
    }



    public void CheckBottomCollision()
    {
        // Sort the shapes by y position
        segments.Sort((a, b) => a.y.CompareTo(b.y));

        // Check collisions
        foreach (ShapeSegment segment in segments)
        {
            // Check if block below is part of this shape
            Block blockBelow = GridManager.instance.GetBlockBelow(segment.x, segment.y);
            if (blockBelow != null && blockBelow.segment != null && segments.Contains(blockBelow.segment))
            {
                segment.SetMoveable(true);
                continue;
            }

            else if (GridManager.instance.CheckForLowerCollision(segment.x, segment.y))
            {
                segment.SetMoveable(false);

                // TODO: Give like a one second window to still move left/right
                DissolveShape();
            }

            else
            {
                segment.SetMoveable(true);
            }
        }
    }

    public void DissolveShape()
    {
        // Take all the segments out of the shape so they can be moved individually
        foreach (ShapeSegment segment in segments)
        {
            segment.transform.parent = null;
        }



        if (GridManager.instance.CheckForLines().Count > 0)
        {
            Debug.Log("Full row detected!");
        }

        GridManager.instance.CheckForGameOver();

        ShapeFactory.instance.StartNewShape();

        // Destroy the shape gameobject
        Destroy(gameObject);

    }



    public void MoveRight()
    {
        // Sort by x position in descending order
        segments.Sort((a, b) => b.x.CompareTo(a.x));

        // Check if any of the blocks in the shape are not moveable
        foreach (ShapeSegment segment in segments)
        {
            if (!segment.canMove) return;
        }

        // Check if any of the blocks in the shape are at the right edge of the grid

        if (CheckRightCollision()) return;

        // Move the shape right
        foreach (ShapeSegment segment in segments)
        {
            segment.Move(segment.x + 1, segment.y);
        }
    }

    public void MoveLeft()
    {
        // Sort by x position in ascending order
        segments.Sort((a, b) => a.x.CompareTo(b.x));

        // Check if any of the blocks in the shape are not moveable
        foreach (ShapeSegment segment in segments)
        {
            if (!segment.canMove) return;
        }

        // Check if any of the blocks in the shape are at the left edge of the grid

        if (CheckLeftCollision()) return;

        // Move the shape left
        foreach (ShapeSegment segment in segments)
        {
            segment.Move(segment.x - 1, segment.y);
        }
    }

    public bool CheckLeftCollision()
    {
        foreach (ShapeSegment segment in segments)
        {
            Block blockLeft = GridManager.instance.GetBlockLeft(segment.x, segment.y);
            if (blockLeft != null && blockLeft.segment != null && segments.Contains(blockLeft.segment))
            {

                return false;
            }

            else if (GridManager.instance.CheckForLeftCollision(segment.x, segment.y))
            {

                return true;
            }
        }

        return false;
    }

    public bool CheckRightCollision()
    {
        foreach (ShapeSegment segment in segments)
        {
            Block blockRight = GridManager.instance.GetBlockRight(segment.x, segment.y);
            if (blockRight != null && blockRight.segment != null && segments.Contains(blockRight.segment))
            {

                return false;
            }

            else if (GridManager.instance.CheckForRightCollision(segment.x, segment.y))
            {

                return true;
            }
        }

        return false;
    }

    public bool IsValidPosition(int x, int y)
    {
        Block block = GridManager.instance.GetBlockAt(x, y);
        if (block == null) return false;
        if (block.isOccupied && !segments.Contains(block.segment)) return false;
        return true;
    }
}
