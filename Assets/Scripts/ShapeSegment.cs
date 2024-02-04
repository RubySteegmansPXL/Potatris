using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShapeSegment : MonoBehaviour
{
    public Sprite sprite;
    public bool canMove { get; private set; } = true;
    public Vector2 position;

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

    private void Update()
    {
        // Always move towards position if not already there
        // Using slerp
        if (transform.localPosition != new Vector3(position.x, position.y, 0) && canMove)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, new Vector3(position.x, position.y, 0), 0.1f);
        }
    }

    public void Create(int x, int y)
    {
        transform.localPosition = new Vector3(x, y, 0);
        GridManager.instance.AttachSegmentToBlock(this, x, y);
        position = new Vector2(x, y);
        ColorSprites();
        FacePicker();
    }

    public void Instantiate(SpriteData spriteData, Sprite[] sprites, Sprite[] faces)
    {
        this.spriteData = spriteData;
        this.sprites = sprites;
        this.faces = faces;
        spriteRenderers[4].sortingOrder = 10; // Face on top
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
                Debug.Log("Face 0", gameObject);
            }
            else
            {
                spriteRenderers[4].sprite = faces[1];
                Debug.Log("Face 1", gameObject);
            }
        }
    }

    [ContextMenu("DeathAnimation")]
    public void DeathAnimation()
    {
        StartCoroutine(DeathAnimationCoroutine());
    }

    IEnumerator DeathAnimationCoroutine()
    {
        canMove = false;

        // Add rigidbody
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();

        // Add collider
        BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();

        // Change sprite layers to high
        foreach (SpriteRenderer rend in spriteRenderers)
        {
            rend.sortingOrder = 12;
        }

        // Change the last one to 13
        spriteRenderers[4].sortingOrder = 13;

        // Add random upward force
        rb.AddForce(new Vector2(Random.Range(-1, 1), 1) * 400);

        // Add random rotation force
        rb.AddTorque(Random.Range(-1, 1) * 30);

        // Slowly shrink the scale
        while (transform.localScale.x > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.02f);
            yield return null;
        }


        Destroy(gameObject);
    }
}
