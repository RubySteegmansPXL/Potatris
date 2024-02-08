using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.ButtonClicked(new CustomEventArgs(gameObject));
            if (GameManager.instance.gameState == GameState.PAUSE)
            {
                // set first object to inactive
                transform.GetChild(0).gameObject.SetActive(false);
                GameManager.instance.ResumeGame();
            }
            else
            {
                // set first object to active
                transform.GetChild(0).gameObject.SetActive(true);
                GameManager.instance.PauseGame();
            }
        }
    }

    public void Resume()
    {
        EventManager.ButtonClicked(new CustomEventArgs(gameObject));
        transform.GetChild(0).gameObject.SetActive(false);
        GameManager.instance.ResumeGame();
    }
}
