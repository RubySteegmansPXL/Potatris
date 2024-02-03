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
    public float defaultMoveSpeed = 1f;
    public float holdMoveSpeed = 0.2f;
    public float timeToHoldBeforeSpeedup = 0.5f;
    public bool canRotate;

    public bool pauseMovement;

    private float timeHeld;
    private float sidewaysMoveTimer;
    private bool isHoldingDown;

    private void Start()
    {
        InvokeRepeating("MoveDown", defaultMoveSpeed, defaultMoveSpeed);
        InvokeRepeating("SpedUpMoveDown", holdMoveSpeed, holdMoveSpeed);
    }

    public void MoveDown()
    {
        if (!isHoldingDown)
            Move(Vector2.down);
    }

    public void SpedUpMoveDown()
    {
        if (isHoldingDown)
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
            Debug.LogWarning("Block is null, so not a valid position.");
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
            if (timeHeld > timeToHoldBeforeSpeedup)
            {
                sidewaysMoveTimer += Time.deltaTime;
                if (sidewaysMoveTimer > holdMoveSpeed)
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


        if (Input.GetKey(KeyCode.DownArrow))
        {
            isHoldingDown = true;
        }
        else
        {
            isHoldingDown = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateShape();
        }
    }

    public void RotateShape()
    {
        if (!canRotate)
        {
            return;
        }

        foreach (ShapeSegment segment in segments)
        {
            if (segment == centerSegment)
            {
                continue;
            }
            Vector2 newPosition = RotateAroundCenter(segment);
            if (newPosition == Vector2.zero)
            {
                Debug.LogWarning("Invalid position, so not rotating.");
                return;
            }
        }

        // TODO: Check for possible nudges

        DetachAllSegments();
        foreach (ShapeSegment segment in segments)
        {
            Vector2 newPosition = RotateAroundCenter(segment);
            segment.position = newPosition;

        }
        AttachAllSegments();

    }

    public Vector2 RotateAroundCenter(ShapeSegment segment, bool clockWise = true)
    {
        if (centerSegment == null)
        {
            Debug.LogWarning("Center segment is null, so not rotating.");
            return Vector2.zero;
        }

        Vector2 center = centerSegment.position;
        Vector2 position = segment.position;

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

        if (!IsValidPosition((int)newPosition.x, (int)newPosition.y))
        {
            return Vector2.zero;
        }

        return newPosition;
    }

    public void Move(Vector2 direction)
    {
        if (pauseMovement)
        {
            return;
        }

        foreach (ShapeSegment segment in segments)
        {
            if (segment == centerSegment)
            {
                continue;
            }

            if (!IsValidPosition((int)segment.position.x + (int)direction.x, (int)segment.position.y + (int)direction.y))
            {
                Debug.LogWarning("Invalid position, so not moving.");
                return;
            }
        }

        DetachAllSegments();
        foreach (ShapeSegment segment in segments)
        {
            segment.position = new Vector2(segment.position.x + direction.x, segment.position.y + direction.y);
        }
        AttachAllSegments();
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
