using System.Collections;
using UnityEngine;

public class PopupMessageManager : MonoBehaviour
{
    public GameObject popupMessagePrefab;
    private Settings settings;

    private int lastY = 0;

    private void Start()
    {
        settings = GameManager.instance.settings;
    }

    private void Awake()
    {
        EventManager.OnFullRow += ShowPopupMessage;
        EventManager.OnTetris += ShowTetrisMessage;
    }

    public void ShowPopupMessage(int y)
    {
        GameObject popupMessage = Instantiate(popupMessagePrefab, new Vector3(-4, y, 0), Quaternion.identity, transform);
        lastY = y;
        popupMessage.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = settings.lineClearMessages[Random.Range(0, settings.lineClearMessages.Length)];
        popupMessage.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = settings.lineClearColors[Random.Range(0, settings.lineClearColors.Length)];
        StartCoroutine(IPopUpAnimation(popupMessage));
    }

    public void ShowTetrisMessage()
    {
        GameObject popupMessage = Instantiate(popupMessagePrefab, new Vector3(-6, lastY, 0), Quaternion.identity, transform);
        popupMessage.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = settings.tetrisMessages[Random.Range(0, settings.tetrisMessages.Length)];
        popupMessage.transform.localScale *= 1.5f;
        popupMessage.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = settings.lineClearColors[Random.Range(0, settings.lineClearColors.Length)];
        StartCoroutine(IPopUpAnimation(popupMessage));
    }

    public IEnumerator IPopUpAnimation(GameObject messageToAnimate)
    {
        float duration = 1.5f; // Duration of the animation in seconds
        float currentTime = 0f;
        Vector3 startScale = Vector3.zero; // Start from scale 0
        Vector3 maxScale = messageToAnimate.transform.localScale; // Original scale as max
        Vector3 startPosition = messageToAnimate.transform.position;
        Vector3 endPosition = startPosition + new Vector3(-1, 4, 0); // Adjust for desired movement
        CanvasGroup canvasGroup = messageToAnimate.GetComponent<CanvasGroup>();
        int rotationDirection = Random.Range(0, 2) * 2 - 1; // Randomly choose rotation direction

        if (canvasGroup == null)
        {
            canvasGroup = messageToAnimate.AddComponent<CanvasGroup>();
        }

        while (currentTime < duration)
        {
            float t = currentTime / duration;
            // Use the animation curve to adjust the scale over time
            float curveValue = settings.lineClearTextAnimationCurve.Evaluate(t);
            messageToAnimate.transform.localScale = Vector3.Lerp(startScale, maxScale, curveValue);
            messageToAnimate.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            messageToAnimate.transform.Rotate(0, 0, 30 * rotationDirection * Time.deltaTime); // Spin
            canvasGroup.alpha = 1 - t; // Fade out

            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(messageToAnimate);
    }
}
