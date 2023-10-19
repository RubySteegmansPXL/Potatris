using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip movementSound;
    public AudioClip gameStartSound;
    public AudioClip gameOverSound;
    public AudioClip fullRownSound;
    public AudioClip blockPlacedSound;
    public AudioClip achievementUnlockedSound;

    
    private void OnEnable()
    {
        EventManager.OnMove += PlayMove;
        EventManager.OnGameStart += PlayGameStart;
        EventManager.OnGameOver += PlayGameOver;
        EventManager.OnFullRow += PlayFullRow;
        EventManager.OnBlockPlaced += PlayBlockPlaced;
        EventManager.OnAchievementUnlocked += PlayAchievementUnlocked;

    }

    void OnDisable() 
    {
        EventManager.OnMove -= PlayMove;
        EventManager.OnGameStart -= PlayGameStart;
        EventManager.OnGameOver -= PlayGameOver;
        EventManager.OnFullRow -= PlayFullRow;
        EventManager.OnBlockPlaced -= PlayBlockPlaced;
        EventManager.OnAchievementUnlocked -= PlayAchievementUnlocked;
    }

    private void PlayMove()
    {
        AudioSource.PlayClipAtPoint(movementSound, transform.position);
    }
    private void PlayGameStart()
    {
        AudioSource.PlayClipAtPoint(gameStartSound, transform.position);
    }
    private void PlayGameOver()
    {
        AudioSource.PlayClipAtPoint(gameOverSound, transform.position);
    }
    private void PlayFullRow()
    {
        AudioSource.PlayClipAtPoint(fullRownSound, transform.position);
    }
    private void PlayBlockPlaced()
    {
        AudioSource.PlayClipAtPoint(blockPlacedSound, transform.position);
    }
    private void PlayAchievementUnlocked()
    {
        AudioSource.PlayClipAtPoint(achievementUnlockedSound, transform.position);
    }
}
