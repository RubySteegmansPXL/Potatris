using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeFactory : MonoBehaviour
{
    public Sprite[] spriteBuildingBlocks;
    public Sprite[] faces;
    public GameObject shapePrefab;
    public List<ShapeData> shapes;

    private Shape shape;
    private SpriteData spriteData;

    public static ShapeFactory instance;
    private Coroutine shapeRoutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateShape();
        }
    }

    public void StartNewShape()
    {
        if (shapeRoutine == null && !GridManager.instance.isResetting)
        {
            shapeRoutine = StartCoroutine(CreateShapeCoroutine());
        }
    }

    IEnumerator CreateShapeCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        CreateShape();
        StopCoroutine(shapeRoutine);
        shapeRoutine = null;
    }

    public void CreateShape()
    {
        shape = Instantiate(shapePrefab, transform.position, Quaternion.identity).GetComponent<Shape>();
        BuildShape();

        shape.SetPosition(4, 17);
    }

    void BuildShape()
    {
        int randomShapeIndex = Random.Range(0, shapes.Count);
        ShapeData shapeData = shapes[randomShapeIndex];

        shape.canRotate = shapeData.canRotate;

        foreach (ShapeSegmentData segment in shapeData.segments)
        {
            Debug.Log(shape.name);
            shape.CreateSegment(segment.x, segment.y, segment.isCenter, shapeData.spriteData, spriteBuildingBlocks, faces);
        }
    }
}
