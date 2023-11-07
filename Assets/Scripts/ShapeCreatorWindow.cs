using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ShapeCreatorWindow : EditorWindow
{
    private ShapeData loadedShapeData; // Field to hold the loaded ShapeData
    private bool[,] grid = new bool[5, 5];
    private bool isRotatable = true;
    private Vector2Int center = new Vector2Int(-1, -1);
    private string shapeName = "NewShape";
    private Color baseColor = Color.white; // Default color for the shape
    private Color accentColor = Color.grey; // Default accent color
    private Color lightColor = Color.clear; // Default light color, can be set to transparent
    private bool useExistingColor = false;
    private bool showColorSettings = true; // Variable to track the foldout state
    private SpriteData existingSpriteData = null; // Field to hold the existing SpriteData
    private bool shapeLoadedSuccessfully = false; // Add this field
    private ShapeData previousLoadedShapeData = null; // Add this field

    private string errorMessage = string.Empty; // Field to store the error message


    [MenuItem("Tetris/Shape Creator")]
    private static void ShowWindow()
    {
        var window = GetWindow<ShapeCreatorWindow>();
        window.titleContent = new GUIContent("Shape Creator");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Load Existing Tetris Shape", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck(); // Start checking for changes
        loadedShapeData = (ShapeData)EditorGUILayout.ObjectField("Existing Shape", loadedShapeData, typeof(ShapeData), false);
        if (EditorGUI.EndChangeCheck()) // Check if the ObjectField has changed
        {
            // If changed, reset the loaded flag and store the new value as previous
            if (previousLoadedShapeData != loadedShapeData)
            {
                shapeLoadedSuccessfully = false;
                previousLoadedShapeData = loadedShapeData;
            }
        }

        if (GUILayout.Button("Load Existing Shape"))
        {
            LoadShape();
        }

        if (shapeLoadedSuccessfully)
        {
            EditorGUILayout.HelpBox("Shape Loaded Successfully!", MessageType.Info);
        }

        // Space
        EditorGUILayout.Space(50);

        GUILayout.Label("Create New Tetris Shape", EditorStyles.boldLabel);
        for (int y = 0; y < 5; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < 5; x++)
            {
                Color color = Color.black; // Default color for empty square
                if (center.x == x && center.y == y)
                    color = Color.green; // Color for the center block
                else if (grid[x, y])
                    color = Color.white; // Color for a filled square

                GUI.backgroundColor = color;
                if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(50)))
                {
                    // Toggle the block state and center
                    ToggleBlockState(x, y);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        // Reset background color for the rest of the UI
        GUI.backgroundColor = Color.white;

        // Add fields for the shape name and colors
        shapeName = EditorGUILayout.TextField("Shape Name", shapeName);
        // Checkbox for using existing color
        useExistingColor = EditorGUILayout.Toggle("Use Existing Color", useExistingColor);

        // Dropdown for color settings
        if (useExistingColor)
        {
            // Field to input an existing SpriteData
            existingSpriteData = (SpriteData)EditorGUILayout.ObjectField("Color Settings", existingSpriteData, typeof(SpriteData), false);
        }
        else
        {
            // Foldout for color fields
            showColorSettings = EditorGUILayout.Foldout(showColorSettings, "Color Settings");
            if (showColorSettings)
            {
                EditorGUI.indentLevel++; // Increase the indent
                baseColor = EditorGUILayout.ColorField("Base Color", baseColor);
                accentColor = EditorGUILayout.ColorField("Accent Color", accentColor);
                lightColor = EditorGUILayout.ColorField("Light Color", lightColor);
                EditorGUI.indentLevel--; // Decrease the indent back to the previous state
            }
        }

        isRotatable = EditorGUILayout.Toggle("Is Rotatable", isRotatable);


        if (GUILayout.Button("Generate Shape"))
        {
            GenerateShape();
        }

        // Red reset button
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Reset"))
        {
            ResetGrid();
        }

        // Display the error message if it's not empty
        if (!string.IsNullOrEmpty(errorMessage))
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
        }
    }
    private void ToggleBlockState(int x, int y)
    {
        // If clicking on the center, unselect it and remove the center designation
        if (center.x == x && center.y == y)
        {
            grid[x, y] = false; // Unselect the square
            center = new Vector2Int(-1, -1); // Remove the center
        }
        else if (grid[x, y])
        {
            // If there's no center, make this block the center
            if (center.x == -1 && center.y == -1)
            {
                center = new Vector2Int(x, y);
            }
            else
            {
                // If there's already a center, and it's not this block, unselect this block
                grid[x, y] = false;
            }
        }
        else
        {
            // If the square is unselected, select it
            grid[x, y] = true;
        }

        // When the user modifies the grid, reset the shape loaded flag
        shapeLoadedSuccessfully = false;
        GUI.changed = true; // Mark the GUI as changed so it will repaint
    }

    private void GenerateShape()
    {
        // Clear the previous error message
        errorMessage = string.Empty;

        // Validation checks
        if (string.IsNullOrWhiteSpace(shapeName))
        {
            errorMessage = "Shape name cannot be empty.";
            return;
        }

        if (center.x == -1 || center.y == -1)
        {
            errorMessage = "No center square selected.";
            return;
        }

        bool hasSegment = false;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y])
                {
                    hasSegment = true;
                    break;
                }
            }

            if (hasSegment)
            {
                break;
            }
        }

        if (!hasSegment)
        {
            errorMessage = "At least one square must be selected to create a shape.";
            return;
        }
        SpriteData spriteDataToUse = null;

        // Check if using existing color and if the provided SpriteData is not null
        if (useExistingColor)
        {
            if (existingSpriteData == null)
            {
                errorMessage = "Please select an existing color.";
                return;
            }
            spriteDataToUse = existingSpriteData;
        }
        else
        {
            // Check if the colors are valid (not completely transparent or default)
            if (baseColor.a == 0 && accentColor.a == 0 && lightColor.a == 0)
            {
                errorMessage = "Please specify at least one color.";
                return;
            }

            // Create new SpriteData with the specified colors
            spriteDataToUse = ScriptableObject.CreateInstance<SpriteData>();
            spriteDataToUse.baseColor = baseColor;
            spriteDataToUse.accentColor = accentColor;
            spriteDataToUse.lightColor = lightColor;

            // Save the new SpriteData asset
            string spriteDataAssetPath = $"Assets/TetrisShapes/{GetValidFileName(shapeName)}_SpriteData.asset";
            AssetDatabase.CreateAsset(spriteDataToUse, spriteDataAssetPath);
        }

        // Proceed to create the shape data
        ShapeData shapeData = ScriptableObject.CreateInstance<ShapeData>();
        shapeData.canRotate = isRotatable;
        List<ShapeSegmentData> segmentList = new List<ShapeSegmentData>();

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if (grid[x, y])
                {
                    ShapeSegmentData segment = new ShapeSegmentData
                    {
                        x = x - center.x,
                        y = y - center.y,
                        isCenter = (x == center.x && y == center.y)
                    };
                    segmentList.Add(segment);
                }
            }
        }

        shapeData.segments = segmentList.ToArray();
        shapeData.spriteData = spriteDataToUse; // Assign the sprite data

        // Save the shape asset
        string shapeAssetPath = $"Assets/TetrisShapes/{GetValidFileName(shapeName)}.asset";
        AssetDatabase.CreateAsset(shapeData, shapeAssetPath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = shapeData;

        // Reset the grid after generating the shape
        ResetGrid();
    }

    private void LoadShape()
    {
        if (loadedShapeData != null)
        {

            errorMessage = string.Empty; // Clear any error message
            // Reset the grid first
            grid = new bool[5, 5];

            // Find the center of the loaded shape to correctly offset the segments
            Vector2Int loadedCenter = new Vector2Int(-1, -1);
            foreach (var segment in loadedShapeData.segments)
            {
                if (segment.isCenter)
                {
                    loadedCenter = new Vector2Int(segment.x, segment.y);
                    break;
                }
            }

            // Make sure we found a center before proceeding
            if (loadedCenter.x == -1 || loadedCenter.y == -1)
            {
                Debug.LogError("Loaded shape does not have a defined center.");
                return;
            }

            // Set the new center for the editor grid
            center = new Vector2Int(2, 2); // Assuming center of the editor grid is at (2, 2)

            // Offset to translate loaded shape coordinates to the editor grid
            int offsetX = center.x - loadedCenter.x;
            int offsetY = center.y - loadedCenter.y;

            // Load the grid
            foreach (ShapeSegmentData segment in loadedShapeData.segments)
            {
                int x = segment.x + offsetX;
                int y = segment.y + offsetY;
                if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                {
                    grid[x, y] = true;
                    if (segment.isCenter)
                    {
                        center = new Vector2Int(x, y);
                    }
                }
                else
                {
                    Debug.LogError($"Segment at ({segment.x}, {segment.y}) is out of bounds after offset. Adjusted position: ({x}, {y})");
                }
            }
            // Set the shape name
            shapeName = loadedShapeData.name;

            // Assuming there is a way to get the colors from the SpriteData, set them here
            if (loadedShapeData.spriteData != null)
            {
                existingSpriteData = loadedShapeData.spriteData; // Reference the existing sprite data
                useExistingColor = true; // Set to use the existing colors
            }
            else
            {
                useExistingColor = false; // Set to use the existing colors
                existingSpriteData = null; // Reference the existing sprite data
            }
            // Set rotatable
            isRotatable = loadedShapeData.canRotate;

            // Force the GUI to refresh
            GUI.changed = true;
            Repaint();
        }
        else
        {
            errorMessage = "No shape data loaded.";
        }

        // At the end of the LoadShape method
        if (!string.IsNullOrEmpty(errorMessage))
        {
            shapeLoadedSuccessfully = false;
        }
        else
        {
            shapeLoadedSuccessfully = true;
        }
    }



    private void ResetGrid()
    {
        grid = new bool[5, 5];
        center = new Vector2Int(-1, -1);
        shapeName = "NewShape"; // Reset the shape name to default
        baseColor = Color.white; // Reset the base color to white
        accentColor = Color.grey; // Reset the accent color to grey
        lightColor = Color.clear; // Reset the light color to clear (transparent)
        existingSpriteData = null; // Reset the existing sprite data reference
        useExistingColor = false; // Reset the toggle for using existing color
        showColorSettings = true; // Reset the foldout to be open
        existingSpriteData = null; // Reset the existing sprite data reference

        errorMessage = string.Empty; // Clear any error message

        // Reset the shape loaded flag
        shapeLoadedSuccessfully = false;

        GUI.changed = true; // Mark the GUI as changed so it will repaint
        Repaint(); // Request the GUI to repaint if necessary
    }


    // Helper method to get a valid file name
    private string GetValidFileName(string name)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
    }
}
