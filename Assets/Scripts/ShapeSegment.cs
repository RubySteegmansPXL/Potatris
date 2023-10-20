using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShapeSegment : MonoBehaviour
{
    public int x;
    public int y;
    public Sprite sprite;
    public bool canMove { get; private set; } = true;


    private SpriteRenderer[] spriteRenderers = new SpriteRenderer[5];
    private SpriteData spriteData;
    private Sprite[] sprites;
    private Sprite[] faces;


    private void Awake()
    {
        spriteRenderers = new SpriteRenderer[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject childObject = new GameObject();
            childObject.transform.parent = transform;
            SpriteRenderer rend = childObject.AddComponent<SpriteRenderer>();
            rend.sortingOrder = 1;
            spriteRenderers[i] = rend;
        }

    }

    public void Create(int x, int y)
    {
        this.x = x;
        this.y = y;

        transform.localPosition = new Vector3(x, y, 0);
        ColorSprites();
        FacePicker();
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

    public void SetMoveable(bool moveable)
    {
        canMove = moveable;
    }
    public void Instantiate(SpriteData spriteData, Sprite[] sprites, Sprite[] faces)
    {
        this.spriteData = spriteData;
        this.sprites = sprites;
        this.faces = faces;
    }

    void ColorSprites()
    {
        spriteRenderers[0].color = spriteData.baseColor;
        spriteRenderers[1].color = spriteData.accentColor;
        spriteRenderers[2].color = spriteData.lightColor;

        for (int i = 0; i < spriteRenderers.Length - 1; i++)
        {
            spriteRenderers[i].sprite = sprites[i];
        }
    }

    void FacePicker()
    {
        if (Random.Range(0, 4) == 1)
        {
            if (Random.Range(0, 2) == 0)
            {
                spriteRenderers[4].sprite = faces[0];
            }
            else
            {
                spriteRenderers[4].sprite = faces[1];
            }
        }
    }
}
