using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuReader : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= GameOver;
    }

    private void GameOver()
    {
        // set first child of this object to active
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
