using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button leaderboardButton;
    public Button tutorialButton;
    public Button achievementsButton;

    public Settings tutorialSettings;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("No GameManager found in scene!");
        }

        startButton.onClick.AddListener(StartButtonClicked);
        quitButton.onClick.AddListener(QuitButtonClicked);
        achievementsButton.onClick.AddListener(AchievementsButtonClicked);
        leaderboardButton.onClick.AddListener(LeaderboardButtonClicked);
        tutorialButton.onClick.AddListener(TutorialButtonClicked);

    }

    private void StartButtonClicked()
    {
        gameManager.StartGame();
    }

    private void QuitButtonClicked()
    {
        FindObjectOfType<SceneFader>().Quit();
    }

    private void AchievementsButtonClicked()
    {
        gameManager.AchievementsPage();
    }

    private void LeaderboardButtonClicked()
    {
        gameManager.LeaderboardPage();
    }

    private void TutorialButtonClicked()
    {
        gameManager.settings = tutorialSettings;
        gameManager.StartTutorial();
    }

}
