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

public enum Language 
{
    [StringValue("en")]
    ENGLISH,

    [StringValue("nl")]
    DUTCH
}

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
sealed class StringValueAttribute : Attribute
{
    public string Value { get; }

    public StringValueAttribute(string value)
    {
        Value = value;
    }
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Language language = Language.ENGLISH; // Default language

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

    public void SetLanguage(string language)
    {
        this.language = (Language)Enum.Parse(typeof(Language), language);
        Debug.Log(this.language);
    }
}
