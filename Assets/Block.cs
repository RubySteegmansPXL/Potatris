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
        this.segment = segment;
        isOccupied = true;
        GetComponent<SpriteRenderer>().color = isOccupied ? Color.red : Color.white;
    }

    public void SetUnoccupied()
    {
        isOccupied = false;
        this.segment = null;
        GetComponent<SpriteRenderer>().color = isOccupied ? Color.red : Color.white;
    }

    public void ChangeToYellow()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }
}
