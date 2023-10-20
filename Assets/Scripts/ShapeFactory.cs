using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeFactory : MonoBehaviour
{
    public Sprite[] spriteBuildingBlocks;
    public Sprite[] faces;
    public GameObject shapePrefab;
    public List<ShapeData> shapes;
    public List<SpriteData> sprites;

    private Shape shape;
    private SpriteData spriteData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateShape();
        }
    }

    void CreateShape()
    {
        shape = Instantiate(shapePrefab, transform.position, Quaternion.identity).GetComponent<Shape>();
        ChooseColor();
        BuildShape();
        shape.SetPosition(5, 15);
    }

    void BuildShape()
    {
        int randomShapeIndex = Random.Range(0, shapes.Count);
        ShapeData shapeData = shapes[randomShapeIndex];

        foreach (ShapeSegmentData segment in shapeData.segments)
        {
            shape.CreateSegment(segment.x, segment.y, segment.isCenter, spriteData, spriteBuildingBlocks, faces);
        }
    }
    void ChooseColor()
    {
        int randomSpriteIndex = Random.Range(0, sprites.Count);
        spriteData = sprites[randomSpriteIndex];
    }
}
