using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPage : MonoBehaviour
{
    public Button exitButton;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("No GameManager found in scene!");
        }

        exitButton.onClick.AddListener(ExitButtonClicked);
    }

    private void ExitButtonClicked()
    {
        gameManager.MainMenu();
    }
}
