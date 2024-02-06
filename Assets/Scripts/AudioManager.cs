using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioClip movementSound;
    public AudioClip moveDownSound;
    public AudioClip rotateSound;
    public AudioClip gameStartSound;
    public AudioClip gameOverSound;
    public AudioClip fullRowSound;
    public AudioClip fullRowSound2;
    public AudioClip tetrisSound;
    public AudioClip tetrisSound2;
    public AudioClip blockPlacedSound;
    public AudioClip achievementUnlockedSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventManager.OnMove += PlayMove;
        EventManager.OnGameStart += PlayGameStart;
        EventManager.OnGameOver += PlayGameOver;
        EventManager.OnFullRow += PlayFullRow;
        EventManager.OnBlockPlaced += PlayBlockPlaced;
        EventManager.OnAchievementUnlocked += PlayAchievementUnlocked;
        EventManager.OnMove += PlayBlockMove;
        EventManager.OnMoveDown += PlayBlockMoveDown;
        EventManager.OnBlockRotate += PlayBlockRotate;
        EventManager.OnTetris += PlayTetris;

    }

    void OnDisable()
    {
        EventManager.OnMove -= PlayMove;
        EventManager.OnGameStart -= PlayGameStart;
        EventManager.OnGameOver -= PlayGameOver;
        EventManager.OnFullRow -= PlayFullRow;
        EventManager.OnBlockPlaced -= PlayBlockPlaced;
        EventManager.OnAchievementUnlocked -= PlayAchievementUnlocked;
        EventManager.OnMove -= PlayBlockMove;
        EventManager.OnMoveDown -= PlayBlockMoveDown;
        EventManager.OnBlockRotate -= PlayBlockRotate;
        EventManager.OnTetris -= PlayTetris;
    }

    private void PlayMove()
    {
        audioSource.PlayOneShot(movementSound);
    }
    private void PlayGameStart()
    {
        audioSource.PlayOneShot(gameStartSound);
    }
    private void PlayGameOver()
    {
        audioSource.PlayOneShot(gameOverSound);
    }
    private void PlayFullRow(int y)
    {
        audioSource.PlayOneShot(fullRowSound);
        audioSource.PlayOneShot(fullRowSound2);
    }

    private void PlayBlockMove()
    {
        audioSource.PlayOneShot(movementSound);
    }

    private void PlayBlockPlaced()
    {
        audioSource.PlayOneShot(blockPlacedSound);
    }

    private void PlayAchievementUnlocked()
    {
        audioSource.PlayOneShot(achievementUnlockedSound);
    }

    private void PlayBlockRotate()
    {
        audioSource.PlayOneShot(rotateSound);
    }

    private void PlayBlockMoveDown()
    {
        audioSource.PlayOneShot(moveDownSound);
    }

    private void PlayTetris()
    {
        PlayFullRow(0);
        audioSource.PlayOneShot(tetrisSound);
        audioSource.PlayOneShot(tetrisSound2);
    }
}
