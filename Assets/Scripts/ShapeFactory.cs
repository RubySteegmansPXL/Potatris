using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeFactory : MonoBehaviour
{
    public Sprite[] spriteBuildingBlocks;
    public Sprite[] faces;
    public GameObject shapePrefab;

    public List<ShapeData> upcomingShapes = new List<ShapeData>();

    private Shape shape;

    public static ShapeFactory instance;

    private Settings settings;
    private Vector2 centerStartingPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);

        settings = GameManager.instance.settings;
        if (settings == null)
        {
            Debug.LogError("Settings not found", gameObject);
        }

        GetCenterStartingPosition();
    }

    private void GetCenterStartingPosition()
    {
        centerStartingPosition = new Vector2(settings.numberOfColumns / 2, settings.numberOfRows - 1);
    }

    private void Start()
    {
        // Generate 3 random shapes to start
        for (int i = 0; i < settings.numberOfShapesInQueue; i++)
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

    public void CreateShape()
    {
        // Get the next shape, remove it from the list, and add a new random shape to the list
        ShapeData nextShape = upcomingShapes[0];
        upcomingShapes.RemoveAt(0);
        upcomingShapes.Add(SelectRandomShape());

        // Build the shape
        BuildShape(nextShape, centerStartingPosition);
    }

    private ShapeData SelectRandomShape()
    {
        int randomShapeIndex = Random.Range(0, settings.possibleShapes.Length);
        ShapeData shapeData = settings.possibleShapes[randomShapeIndex];
        return shapeData;
    }


    void BuildShape(ShapeData nextShape, Vector2 centerBlockPosition)
    {

        if (nextShape == null || nextShape.segments == null)
        {
            Debug.LogError("nextShape or nextShape.segments is null");
            return;
        }

        // Grab the center position of the shape
        int centerSegmentX = nextShape.segments.Where(x => x.isCenter).FirstOrDefault().x;
        // Get the highest y value of the shape
        int highestY = nextShape.segments.OrderByDescending(x => x.y).FirstOrDefault().y;

        // Subtract the highest y value from the center offset, to make sure the shape gets "pushed down" so the entire shape fits on the board
        Vector2 centerOffset = new Vector2(centerSegmentX, highestY);

        Debug.Log("Center Offset: " + centerOffset);


        bool isValidPosition = true;

        foreach (ShapeSegmentData segment in nextShape.segments)
        {
            int spawnPositionX = (int)centerBlockPosition.x + segment.x - (int)centerOffset.x;
            int spawnPositionY = (int)centerBlockPosition.y + segment.y - (int)centerOffset.y;

            // If any of the blocks are occupied, the shape is not in a valid position
            Block currentBlock = GridManager.instance.GetBlockAt(spawnPositionX, spawnPositionY);
            if (currentBlock != null && currentBlock.isOccupied)
            {
                isValidPosition = false;
                break;
            }
        }

        // If the shape is not in a valid position (occupied), game over
        if (!isValidPosition)
        {
            // Game over, topped out.
            Debug.Log("Game Over, topped out.");
            EventManager.GameOver(new CustomEventArgs(gameObject));
            return;
        }

        shape = Instantiate(shapePrefab, Vector3.zero, Quaternion.identity).GetComponent<Shape>();
        shape.canRotate = nextShape.canRotate;

        // Generate the shape around the center position
        foreach (ShapeSegmentData segment in nextShape.segments)
        {
            int spawnPositionX = (int)centerBlockPosition.x + segment.x - (int)centerOffset.x;
            int spawnPositionY = (int)centerBlockPosition.y + segment.y - (int)centerOffset.y;


            shape.CreateSegment(spawnPositionX, spawnPositionY, segment.isCenter, nextShape.spriteData, spriteBuildingBlocks, faces);
        }
    }

    public void Reset()
    {

        DissolveShape();

        upcomingShapes.Clear();
        for (int i = 0; i < settings.numberOfShapesInQueue; i++)
        {
            upcomingShapes.Add(SelectRandomShape());
        }
    }

    public void DissolveShape()
    {
        if (shape != null)
            Destroy(shape.gameObject);
        CreateShape();
    }
}
