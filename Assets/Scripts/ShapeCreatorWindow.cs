using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ShapeCreatorWindow : EditorWindow
{
    private bool[,] grid = new bool[4, 4];
    private bool isRotatable = true;
    private Vector2Int center = new Vector2Int(-1, -1);

    [MenuItem("Tetris/Shape Creator")]
    private static void ShowWindow()
    {
        var window = GetWindow<ShapeCreatorWindow>();
        window.titleContent = new GUIContent("Shape Creator");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Tetris Shape", EditorStyles.boldLabel);

        for (int y = 0; y < 4; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < 4; x++)
            {
                Color color = Color.black; // Default color for empty square
                if (center.x == x && center.y == y)
                    color = Color.green; // Color for the center block
                else if (grid[x, y])
                    color = Color.white; // Color for a filled square

                GUI.backgroundColor = color;
                if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(50)))
                {
                    // Set this block to white or make it the center if it's already white
                    if (center.x == x && center.y == y)
                    {
                        // Clicking on the current center will clear it
                        center = new Vector2Int(-1, -1);
                    }
                    else if (grid[x, y])
                    {
                        // This block is already white, set it to center
                        if (center.x != -1)
                        {
                            // Clear the previous center if there was one
                            grid[center.x, center.y] = false;
                        }
                        center = new Vector2Int(x, y);
                    }
                    else
                    {
                        // The block is black, set it to white
                        grid[x, y] = true;
                    }
                    GUI.changed = true; // Mark the GUI as changed so it will repaint
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;
        isRotatable = EditorGUILayout.Toggle("Is Rotatable", isRotatable);

        if (GUILayout.Button("Generate Shape"))
        {
            GenerateShape();
            ResetGrid();
        }
    }

    private void GenerateShape()
    {
        ShapeData shapeData = ScriptableObject.CreateInstance<ShapeData>();
        shapeData.canRotate = isRotatable;

        List<ShapeSegmentData> segmentList = new List<ShapeSegmentData>(); // Use a list to handle an unknown number of segments

        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                if (grid[x, y])
                {
                    ShapeSegmentData segment = new ShapeSegmentData();
                    segment.x = x - center.x;
                    segment.y = y - center.y;
                    segment.isCenter = (x == center.x && y == center.y);
                    segmentList.Add(segment); // Add to list instead of array
                }
            }
        }

        shapeData.segments = segmentList.ToArray(); // Convert the list to an array now that we have all segments


        AssetDatabase.CreateAsset(shapeData, $"Assets/TetrisShapes/NewShape_{GUID.Generate()}.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = shapeData;
    }

    private void ResetGrid()
    {
        grid = new bool[4, 4];
        center = new Vector2Int(-1, -1);
        Repaint();
    }
}
