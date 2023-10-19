using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSegment : MonoBehaviour
{
    public int x;
    public int y;
    public Sprite sprite;
    public bool canMove { get; private set; } = true;


    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
    }

    public void Create(int x, int y)
    {
        this.x = x;
        this.y = y;

        transform.localPosition = new Vector3(x, y, 0);

        GridManager.instance.CreateBlock(this, x, y);
    }

    public void Move(int x, int y)
    {

        GridManager.instance.MoveBlock(this, this.x, this.y, x, y);

        this.x = x;
        this.y = y;

        transform.localPosition = new Vector3(x, y, 0);
    }

    public void MoveDown()
    {
        GridManager.instance.MoveBlock(this, x, y, x, y - 1);
        Move(x, y - 1);
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetMoveable(bool moveable)
    {
        canMove = moveable;
    }
}
