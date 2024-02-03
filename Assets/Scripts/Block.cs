using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int x;
    public int y;

    public bool isOccupied = false;
    public ShapeSegment segment;

    public void SetBlockPosition(int x, int y)
    {
        this.x = x;
        this.y = y;

        transform.position = new Vector3(x, y, 0);
    }

    private void OnDestroy()
    {
        DetachSegment();
    }

    private void OnDisable()
    {
        DetachSegment();
    }

    public void AttachSegment(ShapeSegment segment)
    {
        this.segment = segment;
        isOccupied = true;
    }

    public void DetachSegment()
    {
        segment = null;
        isOccupied = false;
    }

    public ShapeSegment GetCurrentSegment()
    {
        return segment;
    }
}
