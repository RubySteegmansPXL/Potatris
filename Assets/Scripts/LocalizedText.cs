using UnityEngine;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    public string translationKey;
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    private void OnEnable()
    {
        EventManager.OnLanguageChanged += UpdateText;
    }

    void OnDisable() 
    {
        EventManager.OnMove -= UpdateText;
    }

    public void UpdateText()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = LocalizationManager.Instance.GetTranslation(translationKey);
        }
    }
}
