using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MENU,
    GAME,
    PAUSE,
    GAMEOVER,
    TUTORIAL,
    TUTORIAL_MOVEBLOCK,
    TUTORIAL_TOTALBLOCK,
    TUTORIAL_DONE
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string languageCode = "en"; // Default language
    public Settings settings;

    [HideInInspector] public bool isInTutorial = false;

    [Scene]
    public int mainMenu;
    [Scene]
    public int gameScene;
    [Scene]
    public int tutorialScene;
    [Scene]
    public int achievementsScene;
    [Scene]
    public int leaderboardScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (settings.isTutorial) isInTutorial = true;
        else isInTutorial = false;
        Debug.Log("GAMEMANAGER" + scene.buildIndex);
    }

    public GameState gameState;

    public void StartGame()
    {
        gameState = GameState.GAME;
        EventManager.GameStart(new CustomEventArgs(gameObject));
        FindObjectOfType<SceneFader>().LoadScene(1);
    }

    public void AchievementsPage()
    {
        FindObjectOfType<SceneFader>().LoadScene(achievementsScene);
        Debug.Log("Achievements Page");
    }

    public void PauseGame()
    {
        if (settings.isTutorial) return;
        gameState = GameState.PAUSE;
    }

    public void ResumeGame()
    {
        if (settings.isTutorial) return;
        gameState = GameState.GAME;
    }

    public void GameOver()
    {
        gameState = GameState.GAMEOVER;
    }

    public void MainMenu()
    {
        gameState = GameState.MENU;
        FindObjectOfType<SceneFader>().LoadScene(mainMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetLanguageCode(string languageName)
    {
        // Convert the language name to a language code.
        languageCode = languageName;
        // Call the LanguageChanged event.
        EventManager.LanguageChanged(new CustomEventArgs(gameObject));
    }

    public void StartTutorial()
    {
        settings.isTutorial = true;
        gameState = GameState.TUTORIAL;
        FindObjectOfType<SceneFader>().LoadScene(tutorialScene);
    }

    public void LeaderboardPage()
    {
        FindObjectOfType<SceneFader>().LoadScene(leaderboardScene);
        Debug.Log("Leaderboard Page");
    }
}