using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public List<ShapeSegment> segments;
    public int centerSegmentIndex;
    public Sprite bodySprite;

    public bool isMoving = false;

    public void CreateSegment(int x, int y, bool isCenter)
    {
        ShapeSegment newSegment = new GameObject().AddComponent<ShapeSegment>();
        newSegment.transform.parent = transform;
        newSegment.Create(x, y);
        newSegment.SetSprite(bodySprite);

        segments.Add(newSegment);

        if (isCenter)
        {
            centerSegmentIndex = segments.Count - 1;
        }
    }

    private void Start()
    {
        // Test shape
        CreateSegment(0, 0, false);
        CreateSegment(0, 1, true);
        CreateSegment(0, 2, false);
        CreateSegment(1, 1, false);

        SetPosition(5, 10);
        CheckBottomCollision();
    }

    public void SetPosition(int x, int y)
    {
        transform.position = new Vector3(0, 0, 0);
        // Set the position of the shape according to the center segment
        ShapeSegment centerSegment = segments[centerSegmentIndex];
        int xOffset = x - centerSegment.x;
        int yOffset = y - centerSegment.y;

        foreach (ShapeSegment segment in segments)
        {
            segment.Move(segment.x + xOffset, segment.y + yOffset);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDownSmoothly();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
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
    }

    public void MoveDownSmoothly()
    {
        if (isMoving) return;

        // If any of the blocks in the shape are not moveable, don't move the shape
        foreach (ShapeSegment segment in segments)
        {
            if (!segment.canMove) return;
        }

        StartCoroutine(MoveDownSmoothlyCoroutine());
    }

    private IEnumerator MoveDownSmoothlyCoroutine()
    {
        isMoving = true;
        float time = 0;
        float duration = 0.5f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y - 1, startPosition.z);

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            yield return null;
        }

        transform.position = endPosition;
        isMoving = false;

        // Then, reset own position and change positions of all segments in the shape
        foreach (ShapeSegment segment in segments)
        {
            segment.MoveDown();
        }

        transform.position = new Vector3(0, 0, 0);

        CheckBottomCollision();


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
                Debug.Log("Block below is part of this shape, meaning it will move down with this shape.");
                segment.SetMoveable(true);
                continue;
            }

            else if (GridManager.instance.CheckForLowerCollision(segment.x, segment.y))
            {
                Debug.Log("Collision detected, stopping shape.");
                segment.SetMoveable(false);

                // TODO: Give like a one second window to still move left/right
                DissolveShape();
            }

            else
            {
                Debug.Log("No collision detected.");
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

        // Destroy the shape gameobject
        Destroy(gameObject);

    }

    public void RotateShapeRight()
    {
        // Rotate the shape around the center segment
        // Calculate the offset of each segment from the center segment
        // Then, rotate each segment around the center segment by the offset 90 degrees clockwise
        ShapeSegment centerSegment = segments[centerSegmentIndex];

        foreach (ShapeSegment segment in segments)
        {
            if (segment == centerSegment) continue;

            int xOffset = segment.x - centerSegment.x;
            int yOffset = segment.y - centerSegment.y;

            // Check if we're trying to rotate the shape outside the bounds of the grid
            if (!GridManager.instance.IsInsideBounds(centerSegment.x + yOffset, centerSegment.y - xOffset))
            {
                // TODO: Attempt to move the object left/right to yeet it back onto the grid
                return;
            }

            // Do a collision check on that square to see if we can rotate it
            Block block = GridManager.instance.GetBlockAt(centerSegment.x + yOffset, centerSegment.y - xOffset);
            if (block != null && block.segment != null && !segments.Contains(block.segment))
            {
                Debug.Log("Collision detected, stopping rotation");
                return;
            }
        }

        // Actually move the shape if all the checks pass
        foreach (ShapeSegment segment in segments)
        {
            int xOffset = segment.x - centerSegment.x;
            int yOffset = segment.y - centerSegment.y;

            segment.Move(centerSegment.x + yOffset, centerSegment.y - xOffset);
        }
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
                Debug.Log("Block left is part of this shape, meaning it will move left with this shape.");
                return false;
            }

            else if (GridManager.instance.CheckForLeftCollision(segment.x, segment.y))
            {
                Debug.Log("Collision detected, stopping shape.");
                return true;
            }

            else
            {
                Debug.Log("No collision detected.");
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
                Debug.Log("Block right is part of this shape, meaning it will move right with this shape.");
                return false;
            }

            else if (GridManager.instance.CheckForRightCollision(segment.x, segment.y))
            {
                Debug.Log("Collision detected, stopping shape.");
                return true;
            }

            else
            {
                Debug.Log("No collision detected.");
            }
        }

        return false;
    }
}
