using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShapeSegment : MonoBehaviour
{
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
        transform.localPosition = new Vector3(x, y, 0);
        GridManager.instance.AttachSegmentToBlock(this, x, y);
        ColorSprites();
        FacePicker();
    }

    public void Instantiate(SpriteData spriteData, Sprite[] sprites, Sprite[] faces)
    {
        this.spriteData = spriteData;
        this.sprites = sprites;
        this.faces = faces;
    }

    void ColorSprites()
    {
        if (spriteData != null)
        {
            spriteRenderers[0].color = spriteData.baseColor;
            spriteRenderers[1].color = spriteData.accentColor;
            spriteRenderers[2].color = spriteData.lightColor;
        }
        else
        {
            Debug.LogWarning("SpriteData is null, coloring sprites white.");
            spriteRenderers[0].color = Color.white;
            spriteRenderers[1].color = Color.white;
            spriteRenderers[2].color = Color.white;
        }


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
