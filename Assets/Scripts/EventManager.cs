using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event Action OnMove;
    public static event Action OnMoveDown;
    public static event Action OnMoveLeft;
    public static event Action OnMoveRight;
    public static event Action OnGameStart;
    public static event Action OnGameOver;
    public static event Action OnFullRow;
    public static event Action OnBlockPlaced;
    public static event Action OnAchievementUnlocked;

    public static void Movement()
    {
        OnMove?.Invoke();
    }

    public static void MoveDown()
    {
        OnMoveDown?.Invoke();
    }

    public static void MoveLeft()
    {
        OnMoveLeft?.Invoke();
    }

    public static void MoveRight()
    {
        OnMoveRight?.Invoke();
    }

    public static void GameStart()
    {
        OnGameStart?.Invoke();
    }

    public static void GameOver()
    {
        OnGameOver?.Invoke();
    }
    
    public static void FullRow()
    {
        OnFullRow?.Invoke();
    }

    public static void BlockPlaced()
    {
        OnBlockPlaced?.Invoke();
    }

    public static void AchievementUnlocked()
    {
        OnAchievementUnlocked?.Invoke();
    }
}
