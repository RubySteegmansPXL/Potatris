using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShapeFactory : MonoBehaviour
{
    public Sprite[] spriteBuildingBlocks;
    public Sprite[] faces;
    public GameObject shapePrefab;
    public GameObject previewShapePrefab;

    public List<ShapeData> upcomingShapes = new List<ShapeData>();
    public List<PreviewShape> previewShapes = new List<PreviewShape>();

    private Shape shape;

    public static ShapeFactory instance;

    private Settings settings;
    private Vector2 centerStartingPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        if (settings.isTutorial) return;
        // Generate 3 random shapes to start
        for (int i = 0; i < settings.numberOfShapesInQueue; i++)
        {
            upcomingShapes.Add(SelectRandomShape());
            previewShapes.Add(CreatePreviewShape(settings.numberOfColumns + 2, settings.numberOfRows - (i * 6) - 6, upcomingShapes[i]));
        }

        if (!settings.isTutorial)
        {
            CreateShape();
        }
    }

    public void CreateShape()
    {
        // Get the next shape, remove it from the list, and add a new random shape to the list
        if (upcomingShapes.Count == 0)
        {
            upcomingShapes.Add(SelectRandomShape());
        }
        ShapeData nextShape = upcomingShapes[0];
        upcomingShapes.RemoveAt(0);

        // Remove the corresponding preview shape
        if (previewShapes.Count > 0)
        {
            previewShapes[0].MoveUp();
            previewShapes[0].Shrink();
            previewShapes.RemoveAt(0);
        }

        // Shift existing previews up
        foreach (var previewShape in previewShapes)
        {
            previewShape.MoveUp(); // Adjust this method to ensure it moves previews to the correct position
        }

        if (upcomingShapes.Count < settings.numberOfShapesInQueue && (!settings.isTutorial || GameManager.instance.gameState == GameState.TUTORIAL_DONE))
        {
            for (int i = 0; i < settings.numberOfShapesInQueue - upcomingShapes.Count + 1; i++)
            {
                upcomingShapes.Add(SelectRandomShape());
                PreviewShape newPreview = CreatePreviewShape(settings.numberOfColumns + 2, settings.numberOfRows - (previewShapes.Count * 6) - 12, upcomingShapes.Last());
                previewShapes.Add(newPreview); // Add the new preview shape
                newPreview.Grow();
                newPreview.MoveUp();
            }
        }

        // Build the shape that was next in line
        BuildShape(nextShape, centerStartingPosition);
    }

    private ShapeData SelectRandomShape()
    {
        int randomShapeIndex = Random.Range(0, settings.possibleShapes.Length);
        ShapeData shapeData = settings.possibleShapes[randomShapeIndex];
        return shapeData;
    }


    public void BuildShape(ShapeData nextShape, Vector2 centerBlockPosition, bool dissolveAfterCreation = false)
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
                if (currentBlock == null) Debug.Log("Current block is null");
                if (currentBlock.isOccupied) Debug.Log("Current block is occupied: " + spawnPositionX + ", " + spawnPositionY);
                isValidPosition = false;
                break;
            }
        }


        // If the shape is not in a valid position (occupied), game over
        if (!isValidPosition)
        {
            if (!dissolveAfterCreation && !GameManager.instance.isInTutorial)
            {
                Debug.LogWarning("Game Over, topped out.");
                EventManager.GameOver(new CustomEventArgs(gameObject));
            }
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
        if (dissolveAfterCreation)
        {
            shape.Dissolve();
            Destroy(shape.gameObject);
        }
        else
        {
            EventManager.BlockPlaced(new CustomEventArgs(gameObject), shape);
        }
    }

    public void Reset(bool createNew = true)
    {

        DissolveShape();

        upcomingShapes.Clear();

        if (createNew)
        {
            for (int i = 0; i < settings.numberOfShapesInQueue; i++)
            {
                upcomingShapes.Add(SelectRandomShape());
            }
        }
    }

    public void DissolveShape()
    {
        if (shape != null)
            Destroy(shape.gameObject);
        GridManager.instance.CheckForLine();
        CreateShape();
    }

    // Check if a location is part of the active shape
    public bool IsLocationPartOfActiveShape(Vector2Int location)
    {
        if (shape == null)
            return false;

        foreach (ShapeSegment segment in shape.segments)
        {
            if (segment.position.x == location.x && segment.position.y == location.y)
                return true;
        }

        return false;
    }

    [ContextMenu("TestPreviewShape")]
    public void TestPreviewShape()
    {
        CreatePreviewShape(settings.numberOfColumns + 2, settings.numberOfRows - 6, upcomingShapes[0]);
        CreatePreviewShape(settings.numberOfColumns + 2, settings.numberOfRows - 12, upcomingShapes[1]);
        CreatePreviewShape(settings.numberOfColumns + 2, settings.numberOfRows - 18, upcomingShapes[2]);

    }

    public void RotateActiveShape()
    {
        if (shape != null)
        {
            shape.RotateShape();
        }
    }

    public void MoveActiveShape(Vector2Int direction)
    {
        if (shape != null)
        {
            shape.Move(direction);
        }
    }

    public PreviewShape CreatePreviewShape(int x, int y, ShapeData shapeData)
    {
        if (shapeData == null || shapeData.segments == null)
        {
            Debug.LogError("shapeData or shapeData.segments is null");
            return null;
        }

        // Instantiate the shape at the specified world position
        PreviewShape previewShape = Instantiate(previewShapePrefab, Vector3.zero, Quaternion.identity).GetComponent<PreviewShape>();

        // Generate the shape segments
        foreach (ShapeSegmentData segment in shapeData.segments)
        {
            // For preview, we ignore the grid and just place segments relative to the given x and y
            int segmentX = x + segment.x;
            int segmentY = y + segment.y;

            // Create segment without considering block business, simply using world coordinates
            previewShape.CreateSegment(segmentX, segmentY, shapeData.spriteData, spriteBuildingBlocks, faces);
        }

        return previewShape;
    }

    public void DestroyShapeImmediate()
    {
        if (shape != null)
        {
            shape.DetachAllSegments();
            Destroy(shape.gameObject);
        }
    }

}
