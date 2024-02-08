using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementTracker : MonoBehaviour
{
    private int consecutiveTetrises = 0;
    private int quickDrops = 0;
    private bool lineClearedThisGame = false;
    private int score = 0;
    private bool isHolding = false;
    private bool fiveHundredThisGame = false;

    private void OnEnable()
    {
        EventManager.OnTetris += HandleTetris;
        EventManager.OnFullRow += HandleFullRow;
        EventManager.OnBlockRotate += HandleBlockRotate;
        EventManager.OnMoveDown += HandleMoveDown;
        EventManager.OnGameStart += HandleGameStart;
        EventManager.OnScoreUpdates += HandleScoreUpdates;
        EventManager.OnLanguageChanged += HandleLanguageChanged;
        EventManager.OnBlockPlaced += HandleBlockPlaced;
        // Initialize other event subscriptions
    }

    private void OnDisable()
    {
        // Unsubscribe from all events
        // Similar to OnEnable, but using -= operator
        EventManager.OnTetris -= HandleTetris;
        EventManager.OnFullRow -= HandleFullRow;
        EventManager.OnBlockRotate -= HandleBlockRotate;
        EventManager.OnMoveDown -= HandleMoveDown;
        EventManager.OnGameStart -= HandleGameStart;
        EventManager.OnLanguageChanged -= HandleLanguageChanged;
        EventManager.OnBlockPlaced -= HandleBlockPlaced;
    }

    private void HandleGameStart()
    {
        consecutiveTetrises = 0;
        quickDrops = 0;
        Debug.Log("Game started, achievement");
    }

    private void HandleMoveDown(bool isHolding)
    {
        this.isHolding = isHolding;
    }
    private void HandleBlockPlaced(Shape shape)
    {
        if (isHolding)
        {
            quickDrops++;
            if (quickDrops >= 10)
            {
                AchievementsManager.instance.UpdateAchievement("ach_4", 100);
            }
            AchievementsManager.instance.UpdateAchievement("ach_3", 2);
        }
    }
    private void HandleLanguageChanged()
    {
        Debug.Log("Language changed, achievement");
        AchievementsManager.instance.UpdateAchievement("ach_7", 100);    
    }

    private void HandleTetris()
    {
        consecutiveTetrises++;
        if (consecutiveTetrises >= 3)
        {
            AchievementsManager.instance.UpdateAchievement("ach_10", 100);
        }
    }

    private void HandleFullRow(int y, int rowsCleared)
    {
        if(rowsCleared != 4)
        {
            consecutiveTetrises = 0;
        }

        if(lineClearedThisGame == false)
        {
            lineClearedThisGame = true;
            AchievementsManager.instance.UpdateAchievement("ach_1", 20);
        }

        AchievementsManager.instance.UpdateAchievement("ach_2", 2 * rowsCleared);
    }

    private void HandleBlockRotate()
    {
        AchievementsManager.instance.UpdateAchievement("ach_6", 1);
    }

    private void HandleScoreUpdates(int scoreAdded)
    {
        score += scoreAdded;
        if(score >= 500 && !fiveHundredThisGame)
        {
            AchievementsManager.instance.UpdateAchievement("ach_5", 20);
            fiveHundredThisGame = true;
        }
        AchievementsManager.instance.UpdateAchievement("ach_8", scoreAdded / 50);
        AchievementsManager.instance.UpdateAchievement("ach_9", scoreAdded / 200);
    }

    // Additional methods as needed for other achievements
}
