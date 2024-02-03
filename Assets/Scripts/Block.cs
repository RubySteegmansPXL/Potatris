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
        if (isOccupied)
        {
            Debug.LogWarning("Tried to attach segment to an occupied block.");

        }
        this.segment = segment;
        isOccupied = true;
    }

    public void DetachSegment()
    {
        segment = null;
        isOccupied = false;
    }

    public void Reset()
    {
        if (segment != null)
            Destroy(segment.gameObject);
        DetachSegment();
    }

    public ShapeSegment GetCurrentSegment()
    {
        return segment;
    }

    private void OnDrawGizmos()
    {
        if (segment != null)
            Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
