using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeFactory : MonoBehaviour
{
    public Sprite[] spriteBuildingBlocks;
    public Sprite[] faces;
    public GameObject shapePrefab;
    public List<ShapeData> shapes = new List<ShapeData>();

    public List<ShapeData> upcomingShapes = new List<ShapeData>();

    private Shape shape;
    private SpriteData spriteData;

    public static ShapeFactory instance;
    private Coroutine shapeRoutine;

    public Vector2 centerStartingPosition = new Vector2(4, 17);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }

    private void Start()
    {
        // Generate 3 random shapes to start
        for (int i = 0; i < 3; i++)
        {
            upcomingShapes.Add(SelectRandomShape());
        }
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
        // Spawn an empty shape (shape prefab)
        shape = Instantiate(shapePrefab, Vector3.zero, Quaternion.identity).GetComponent<Shape>();

        // Get the next shape, remove it from the list, and add a new random shape to the list
        ShapeData nextShape = upcomingShapes[0];
        upcomingShapes.RemoveAt(0);
        upcomingShapes.Add(SelectRandomShape());

        // Build the shape
        BuildShape(nextShape, centerStartingPosition);
    }

    private ShapeData SelectRandomShape()
    {
        int randomShapeIndex = Random.Range(0, shapes.Count);
        ShapeData shapeData = shapes[randomShapeIndex];
        return shapeData;
    }


    void BuildShape(ShapeData nextShape, Vector2 centerBlockPosition)
    {

        // Grab the center position of the shape
        int centerSegmentX = nextShape.segments.Where(x => x.isCenter).FirstOrDefault().x;
        // Get the highest y value of the shape
        int highestY = nextShape.segments.OrderByDescending(x => x.y).FirstOrDefault().y;

        // Subtract the highest y value from the center offset, to make sure the shape gets "pushed down" so the entire shape fits on the board
        Vector2 centerOffset = new Vector2(centerSegmentX, highestY);




        shape.canRotate = nextShape.canRotate;

        Debug.Log("Center Offset: " + centerOffset);
        Debug.Log("Center Spawn location: " + (centerBlockPosition - centerOffset));

        // Generate the shape around the center position
        foreach (ShapeSegmentData segment in nextShape.segments)
        {
            shape.CreateSegment((int)centerBlockPosition.x + segment.x - (int)centerOffset.x, (int)centerBlockPosition.y + segment.y - (int)centerOffset.y, segment.isCenter, nextShape.spriteData, spriteBuildingBlocks, faces);
        }
    }
}
