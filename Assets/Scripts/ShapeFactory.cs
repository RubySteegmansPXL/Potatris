using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeFactory : MonoBehaviour
{
    public Sprite[] spriteBuildingBlocks;
    public Sprite[] faces;
    public List<ShapeData> shapes;
    public List<SpriteData> sprites;

    private Shape shape;
    private SpriteData spriteData;
    private SpriteRenderer[] spriteRenderers = new SpriteRenderer[5];

    void CreateShape()
    {
        shape = new Shape();
        ChooseColor();
        ColorSprites();
        BuildShape();
    }

    void BuildShape()
    {
        int randomShapeIndex = Random.Range(0, shapes.Count);
        ShapeData shapeData = shapes[randomShapeIndex];
        
        foreach(ShapeSegmentData segment in shapeData.segments)
        {
            FacePicker();
            shape.CreateSegment(segment.x, segment.y, segment.isCenter, spriteRenderers);
        }
    }

    void ChooseColor()
    {
        int randomSpriteIndex = Random.Range(0, sprites.Count);
        spriteData = sprites[randomSpriteIndex];
    }

    void ColorSprites()
    {
        spriteRenderers[0].color = spriteData.baseColor;
        spriteRenderers[1].color = spriteData.accentColor;
        spriteRenderers[2].color = spriteData.lightColor;

        for (int i = 0; i < spriteRenderers.Length - 1; i++)
        {
            spriteRenderers[i].sprite = spriteBuildingBlocks[i];
        }
    }

    void FacePicker()
    {
        if(Random.Range(0, 4) == 1)
        {
            if(Random.Range(0, 2) == 0)
            {
                spriteRenderers[4].sprite = spriteBuildingBlocks[4];
            } else
            {
                spriteRenderers[4].sprite = spriteBuildingBlocks[5];
            }
        }
    }
}
