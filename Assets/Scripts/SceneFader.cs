using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public AnimationCurve fadeCurve;
    public float fadeTime = 1f;

    public static SceneFader instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            StartFadeIn();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }



    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public void MainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    public void Quit()
    {
        StartCoroutine(QuitGame());
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneRoutine(sceneIndex));
    }


    IEnumerator FadeOut()
    {
        // Start fade
        float t = 0f;

        // For as long as t is less than the fade time, keep fading out
        while (t < fadeTime)
        {
            float a = fadeCurve.Evaluate(t / fadeTime);
            fadeImage.color = new Color(231f, 243f, 230f, a);
            t += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        // Start fade
        float t = 1f;

        // For as long as t is less than the fade time, keep fading out
        while (t > 0f)
        {
            float a = fadeCurve.Evaluate(t / fadeTime);
            fadeImage.color = new Color(231f, 243f, 230f, a);
            t -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator LoadMainMenu()
    {
        yield return FadeOut();
        SceneManager.LoadScene(0);
    }

    IEnumerator QuitGame()
    {
        yield return FadeOut();
        Application.Quit();
    }

    IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        yield return FadeOut();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
