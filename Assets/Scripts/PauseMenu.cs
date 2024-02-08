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

            if (GameManager.instance.isInTutorial)
            {
                if (transform.GetChild(0).gameObject.activeSelf)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            else
            {
                if (transform.GetChild(0).gameObject.activeSelf)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    GameManager.instance.ResumeGame();
                }
                else
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    GameManager.instance.PauseGame();
                }
            }
        }
    }

    public void Resume()
    {
        EventManager.ButtonClicked(new CustomEventArgs(gameObject));
        transform.GetChild(0).gameObject.SetActive(false);

        if (!GameManager.instance.isInTutorial)
        {
            GameManager.instance.ResumeGame();
        }
    }
}
