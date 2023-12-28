using UnityEngine;
using TMPro;

/// <summary>
/// A component that allows dynamic localization of TextMeshPro text.
/// </summary>
public class LocalizedText : MonoBehaviour
{
    public string translationKey; // The key used to retrieve the localized text. (e.g. "main_menu_credits")
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
        EventManager.OnLanguageChanged -= UpdateText;
    }

    /// <summary>
    /// Updates the text content based on the current language and translation key.
    /// </summary>
    public void UpdateText()
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = LocalizationManager.Instance.GetTranslation(translationKey);
        }
    }
}
