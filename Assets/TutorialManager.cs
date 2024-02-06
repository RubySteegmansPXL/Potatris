using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NaughtyAttributes;
using Unity.VisualScripting;
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

    public ShapeData tutorialShapeForLineFill;

    public ShapeData[] tutorialShapes;
    public Vector2Int[] tutorialPositions;

    private bool blockPlaced = false;

    private void Start()
    {

        GameManager.instance.gameState = GameState.TUTORIAL;


        StartCoroutine(IStartTutorial());
    }

    private void OnEnable()
    {
        EventManager.OnBlockPlaced += OnPlaceBlock;
    }

    private void OnDisable()
    {
        EventManager.OnBlockPlaced -= OnPlaceBlock;
    }

    public void OnPlaceBlock()
    {
        blockPlaced = true;
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
        GameManager.instance.gameState = GameState.TUTORIAL_USERBLOCK;
        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_basic_movement_key);
        yield return new WaitForSeconds(7.1f);

        GameManager.instance.gameState = GameState.TUTORIAL;
        EventManager.FullRow(new CustomEventArgs(gameObject), 13);
        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_fast_movement_key);
        while (!Input.GetKeyDown(KeyCode.DownArrow))
        {
            yield return null;
        }
        GameManager.instance.gameState = GameState.TUTORIAL_USERBLOCK;
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

        while (!blockPlaced)
        {
            yield return null;
        }

        blockPlaced = false;

        GameManager.instance.gameState = GameState.TUTORIAL;
        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_rotate_key);

        // Check for rotation
        while (!Input.GetKeyDown(KeyCode.UpArrow))
        {
            yield return null;
        }

        GameManager.instance.gameState = GameState.GAME;

        while (!blockPlaced)
        {
            yield return null;
        }

        blockPlaced = false;

        GameManager.instance.gameState = GameState.TUTORIAL;

        GridManager.instance.ResetGrid();

        tutorialText.text = LocalizationManager.Instance.GetTranslation(tutorial_lineclear_key);

        ShapeFactory.instance.DestroyShapeImmediate();

        // Spawn blocks
        for (int i = 0; i < tutorialShapes.Length; i++)
        {
            ShapeFactory.instance.BuildShape(tutorialShapes[i], tutorialPositions[i], true);
            EventManager.MovementDown(new CustomEventArgs(gameObject));
            yield return new WaitForSeconds(0.2f);
        }

        ShapeFactory.instance.BuildShape(tutorialShapeForLineFill, new Vector2Int(5, 18));

        GameManager.instance.gameState = GameState.TUTORIAL_USERBLOCK;

        blockPlaced = false;

        // Check for line clear
        while (!blockPlaced)
        {
            yield return null;
        }

        GameManager.instance.gameState = GameState.GAME;

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
