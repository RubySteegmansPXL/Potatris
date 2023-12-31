using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public int Score { get; private set; }
    public int Level { get; private set; }

    public TextMeshProUGUI scoreText;

    private bool lastClearWasTetris;

    private void OnEnable()
    {
        EventManager.OnFullRow += FullRow;
    }

    private void Start()
    {
        Level = 1;
        Score = 0;
        scoreText.text = Score.ToString();
    }
    private void OnDisable()
    {
        EventManager.OnFullRow -= FullRow;
    }

    private void FullRow(int lines)
    {
        int baseScore;
        switch (lines)
        {
            case 1:
                baseScore = 40;
                lastClearWasTetris = false;
                break;
            case 2:
                baseScore = 100;
                lastClearWasTetris = false;
                break;
            case 3:
                baseScore = 300;
                lastClearWasTetris = false;
                break;
            case 4:
                baseScore = lastClearWasTetris ? 1200 : 800;
                lastClearWasTetris = true;
                break;
            default:
                baseScore = 0;
                lastClearWasTetris = false;
                break;
        }
        Score += baseScore * Level;
        scoreText.text = Score.ToString();
    }
}
