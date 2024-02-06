using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int x;
    public int y;

    public bool isOccupied
    {
        get
        {
            return segment != null;
        }
    }
    public ShapeSegment segment;

    public void SetBlockPosition(int x, int y)
    {
        this.x = x;
        this.y = y;

        transform.localPosition = new Vector3(x, y, 0);
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
        // Double check for missing segments
        if (this.segment == null)
        {
            DetachSegment();
        }

        if (isOccupied)
        {
            Debug.LogWarning("Tried to attach segment to an occupied block.");

        }
        this.segment = segment;
    }

    public void DetachSegment()
    {
        segment = null;
    }

    public void Reset()
    {
        if (segment != null)
        {
            // Test to see if we can do a funny
            //Destroy(segment.gameObject);

            segment.DeathAnimation();
        }
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
