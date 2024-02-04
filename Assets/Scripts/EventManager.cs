using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action OnMove;
    public static event Action OnMoveDown;
    public static event Action OnGameStart;
    public static event Action OnGameOver;
    public static event Action<int> OnFullRow;
    public static event Action OnBlockPlaced;
    public static event Action OnAchievementUnlocked;
    public static event Action OnGridCreate;
    public static event Action OnGridUpdate;
    public static event Action OnLanguageChanged;
    public static event Action OnBlockRotate;

    public static void Movement(CustomEventArgs e)
    {
        OnMove?.Invoke();
    }

    public static void MovementDown(CustomEventArgs e)
    {
        OnMoveDown?.Invoke();
    }

    public static void GameStart(CustomEventArgs e)
    {
        OnGameStart?.Invoke();
    }

    public static void GameOver(CustomEventArgs e)
    {
        OnGameOver?.Invoke();
    }

    public static void FullRow(CustomEventArgs e, int lines)
    {
        OnFullRow?.Invoke(lines);
        Debug.Log("FullRow called by " + e.Sender.name, e.Sender);
    }

    public static void BlockPlaced(CustomEventArgs e)
    {
        OnBlockPlaced?.Invoke();
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

}


public class CustomEventArgs : EventArgs
{
    public GameObject Sender { get; }

    public CustomEventArgs(GameObject sender)
    {
        Sender = sender;
    }
}
