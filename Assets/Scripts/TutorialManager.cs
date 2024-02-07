using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI tutorialText;
    public GameObject tutorialPanel;

    public string tutorial_welcome_key = "tutorial_welcome";
    public string tutorial_basic_movement_key = "tutorial_basic_movement";
    public string tutorial_fast_movement_key = "tutorial_fast_movement";
    public string tutorial_side_movement_key = "tutorial_side_movement";
    public string tutorial_rotate_key = "tutorial_rotate";
    public string tutorial_lineclear_key = "tutorial_lineclear";
    public string tutorial_final_key = "tutorial_final";
    public string tutorial_tetris_key = "tutorial_tetris";
    public string tutorial_preview_key = "tutorial_preview";

    public ShapeData tutorialShapeForLineFill;
    public ShapeData bigboi;

    public ShapeData[] tutorialShapes;
    public Vector2Int[] tutorialPositions;

    private bool blockPlaced = false;
    private bool lineCleared = false;

    private void Start()
    {

        GameManager.instance.gameState = GameState.TUTORIAL;


        StartCoroutine(IStartTutorial());
    }

    private void OnEnable()
    {
        EventManager.OnBlockPlaced += OnPlaceBlock;
        EventManager.OnFullRow += OnLineCleared;
    }


    private void OnDisable()
    {
        EventManager.OnBlockPlaced -= OnPlaceBlock;
        EventManager.OnFullRow -= OnLineCleared;
    }

    public void OnPlaceBlock()
    {
        blockPlaced = true;
    }

    public void OnLineCleared(int y)
    {
        lineCleared = true;
    }

    public IEnumerator IStartTutorial()
    {
        EventManager.FullRow(new CustomEventArgs(gameObject), 16);
        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_welcome_key);
        ShapeFactory.instance.CreateShape();
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        GameManager.instance.gameState = GameState.TUTORIAL_TOTALBLOCK;
        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_basic_movement_key);
        yield return new WaitForSeconds(7.1f);

        GameManager.instance.gameState = GameState.TUTORIAL;
        EventManager.FullRow(new CustomEventArgs(gameObject), 13);
        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_fast_movement_key);
        while (!Input.GetKeyDown(KeyCode.DownArrow))
        {
            yield return null;
        }
        GameManager.instance.gameState = GameState.TUTORIAL_MOVEBLOCK;
        blockPlaced = false;

        // Wait until the event onblockplaced is called
        while (!blockPlaced)
        {
            yield return null;
        }

        blockPlaced = false;
        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_side_movement_key);
        GameManager.instance.gameState = GameState.TUTORIAL;

        // Check for left and right movement
        while (!Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow))
        {
            yield return null;
        }

        GameManager.instance.gameState = GameState.GAME;

        // Move the active shape to the left or right
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ShapeFactory.instance.MoveActiveShape(Vector2Int.left);
        }
        else
        {
            ShapeFactory.instance.MoveActiveShape(Vector2Int.right);
        }

        while (!blockPlaced)
        {
            yield return null;
        }

        blockPlaced = false;

        GameManager.instance.gameState = GameState.TUTORIAL;
        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_rotate_key);

        ShapeFactory.instance.DestroyShapeImmediate();
        ShapeFactory.instance.BuildShape(tutorialShapes[0], new Vector2Int(5, 15));

        Debug.Log("Waiting for rotation");

        // Check for rotation
        while (!Input.GetKeyDown(KeyCode.UpArrow))
        {
            yield return null;
        }

        GameManager.instance.gameState = GameState.GAME;

        Debug.Log("Rotated the shape");
        ShapeFactory.instance.RotateActiveShape();


        blockPlaced = false;

        while (!blockPlaced)
        {
            yield return null;
        }

        blockPlaced = false;

        GameManager.instance.gameState = GameState.TUTORIAL;

        GridManager.instance.ResetGrid(false);

        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_lineclear_key);

        ShapeFactory.instance.DestroyShapeImmediate();
        yield return null;

        // Spawn blocks
        for (int i = 0; i < tutorialShapes.Length; i++)
        {
            ShapeFactory.instance.BuildShape(tutorialShapes[i], tutorialPositions[i], true);
            EventManager.MovementDown(new CustomEventArgs(gameObject));
            yield return new WaitForSeconds(0.2f);
        }

        Debug.LogWarning("Built shapes, now building line fill shape");

        yield return null;
        ShapeFactory.instance.BuildShape(tutorialShapeForLineFill, new Vector2Int(5, 18));

        GameManager.instance.gameState = GameState.TUTORIAL_MOVEBLOCK;

        blockPlaced = false;

        lineCleared = false;
        // Check for line clear
        while (!lineCleared)
        {
            yield return null;
        }


        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_tetris_key);

        GridManager.instance.ResetGrid(false);

        ShapeFactory.instance.BuildShape(bigboi, new Vector2Int(0, 4), true);
        yield return new WaitForSeconds(0.3f);
        ShapeFactory.instance.BuildShape(bigboi, new Vector2Int(6, 4), true);
        yield return new WaitForSeconds(0.3f);
        ShapeFactory.instance.BuildShape(tutorialShapeForLineFill, new Vector2Int(5, 17));

        GameManager.instance.gameState = GameState.TUTORIAL_MOVEBLOCK;


        lineCleared = false;
        while (!lineCleared)
        {
            yield return null;
        }

        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_preview_key);

        ShapeFactory.instance.DestroyShapeImmediate();
        yield return null;
        yield return new WaitForSeconds(0.5f);
        ShapeFactory.instance.CreateShape();

        GameManager.instance.gameState = GameState.TUTORIAL_DONE;

        blockPlaced = false;

        while (!blockPlaced)
        {
            yield return null;
        }

        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_final_key);
    }

    [Button]
    public void TestShapeSummon()
    {
        // Spawn blocks
        for (int i = 0; i < tutorialShapes.Length; i++)
        {
            ShapeFactory.instance.BuildShape(tutorialShapes[i], tutorialPositions[i], true);
        }
    }

    [Button]
    public void Clear()
    {
        GridManager.instance.ResetGrid();
    }
}
