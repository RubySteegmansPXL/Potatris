using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MENU,
    GAME,
    PAUSE,
    GAMEOVER
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string languageCode = "en"; // Default language

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
        FindObjectOfType<SceneFader>().LoadScene(2);
        Debug.Log("Achievements Page");
    }

    public void PauseGame()
    {
        gameState = GameState.PAUSE;
    }

    public void ResumeGame()
    {
        gameState = GameState.GAME;
    }

    public void GameOver()
    {
        gameState = GameState.GAMEOVER;
    }

    public void MainMenu()
    {
        gameState = GameState.MENU;
        FindObjectOfType<SceneFader>().LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetLanguageCode(string languageName)
    {
        // Convert the language name to a language code.
        languageCode = LanguageManager.Instance.GetLanguageCode(languageName);
        // Call the LanguageChanged event.
        EventManager.LanguageChanged(new CustomEventArgs(gameObject));
    }
}