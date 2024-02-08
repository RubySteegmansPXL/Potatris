using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action OnMove;
    public static event Action<bool> OnMoveDown;
    public static event Action OnGameStart;
    public static event Action OnGameOver;
    public static event Action<int, int> OnFullRow; // Event with an integer argument corresponding to the y value (height) of the row that was cleared
    public static event Action OnTetris;
    public static event Action<Shape> OnBlockPlaced;
    public static event Action OnAchievementUnlocked;
    public static event Action OnGridCreate;
    public static event Action OnGridUpdate;
    public static event Action OnLanguageChanged;
    public static event Action OnBlockRotate;
    public static event Action<int> OnScoreUpdates;
    public static event Action OnFadeIn;
    public static event Action OnFadeOut;

    public static void Movement(CustomEventArgs e)
    {
        OnMove?.Invoke();
    }

    public static void MovementDown(CustomEventArgs e, bool isHolding)
    {
        OnMoveDown?.Invoke(isHolding);
    }

    public static void GameStart(CustomEventArgs e)
    {
        OnGameStart?.Invoke();
    }

    public static void GameOver(CustomEventArgs e)
    {
        OnGameOver?.Invoke();
    }

    public static void FullRow(CustomEventArgs e, int height, int lines)
    {
        OnFullRow?.Invoke(height, lines);
        Debug.Log("FullRow called by " + e.Sender.name, e.Sender);
    }

    public static void BlockPlaced(CustomEventArgs e, Shape shape)
    {
        OnBlockPlaced?.Invoke(shape);
    }

    public static void AchievementUnlocked(CustomEventArgs e)
    {
        OnAchievementUnlocked?.Invoke();
    }

    public static void GridCreate(CustomEventArgs e)
    {
        OnGridCreate?.Invoke();
        Debug.Log("GridCreate called by " + e.Sender.name, e.Sender);
    }

    public static void GridUpdate(CustomEventArgs e)
    {
        OnGridUpdate?.Invoke();
    }

    public static void LanguageChanged(CustomEventArgs e)
    {
        OnLanguageChanged?.Invoke();
    }

    public static void BlockRotate(CustomEventArgs e)
    {
        OnBlockRotate?.Invoke();
    }

    public static void Tetris(CustomEventArgs e)
    {
        OnTetris?.Invoke();
    }

    public static void ScoreUpdates(CustomEventArgs e, int scoreAdded)
    {
        OnScoreUpdates?.Invoke(scoreAdded);
    }

    public static void FadeIn(CustomEventArgs e)
    {
        OnFadeIn?.Invoke();
    }

    public static void FadeOut(CustomEventArgs e)
    {
        OnFadeOut?.Invoke();
    }

}


public class CustomEventArgs : EventArgs
{
    public GameObject Sender { get; }

    public CustomEventArgs(GameObject sender)
    {
        Sender = sender;
    }
}
