using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameState gameState;

    public void StartGame()
    {
        gameState = GameState.GAME;
        EventManager.GameStart(new CustomEventArgs(gameObject));
        FindObjectOfType<SceneFader>().LoadScene(1);
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