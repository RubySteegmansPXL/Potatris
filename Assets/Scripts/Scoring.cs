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
    private bool isHoldingDown;

    private void OnEnable()
    {
        EventManager.OnFullRow += FullRow;
        EventManager.OnMoveDown += MovingDown;
        EventManager.OnBlockPlaced += BlockPlaced;
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
    void MovingDown(bool isHolding)
    {
        isHoldingDown = isHolding;
    }
    private void BlockPlaced(Shape shape)
    {
        if(isHoldingDown)
        {
            Score += shape.segments.Count * 2;
            scoreText.text = Score.ToString();
            EventManager.ScoreUpdates(new CustomEventArgs(gameObject), shape.segments.Count * 2);
        } else
        {
            Score += shape.segments.Count;
            scoreText.text = Score.ToString();
            EventManager.ScoreUpdates(new CustomEventArgs(gameObject), shape.segments.Count);
        }
    }
    private void FullRow(int y,  int lines)
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
        EventManager.ScoreUpdates(new CustomEventArgs(gameObject), baseScore * Level);
    }
}
