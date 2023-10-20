using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int x;
    public int y;

    public bool isOccupied = false;
    public ShapeSegment segment;

    public Block(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;

        transform.position = new Vector3(x, y, 0);
    }

    public void UpdatePosition()
    {
        transform.position = new Vector3(x, y, 0);
    }

    public void SetOccupied(ShapeSegment segment)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        this.segment = segment;
        isOccupied = true;
    }

    public void SetUnoccupied()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        isOccupied = false;
        this.segment = null;
    }

    public void MoveDownSegment()
    {
        if (segment != null)
        {
            segment.MoveDown();
        }
    }

    public void SetLine()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        if (segment != null)
        {
            Destroy(segment.gameObject);
            segment = null;
        }
    }
}
