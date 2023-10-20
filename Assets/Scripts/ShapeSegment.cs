using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSegment : MonoBehaviour
{
    public int x;
    public int y;
    public Sprite sprite;
    public bool canMove { get; private set; } = true;


    private SpriteRenderer[] spriteRenderers;
    
    private void Awake()
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i] = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderers[i].sortingOrder = 1;
        }
    }

    public void Create(int x, int y)
    {
        this.x = x;
        this.y = y;

        transform.localPosition = new Vector3(x, y, 0);
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

    public void SetSprites(SpriteRenderer[] spriteRenderers)
    {
        this.spriteRenderers = spriteRenderers;
    }

    public void SetMoveable(bool moveable)
    {
        canMove = moveable;
    }
}
