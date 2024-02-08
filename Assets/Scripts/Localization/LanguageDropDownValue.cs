using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages the dropdown menu for language selection.
/// </summary>
public class LanguageDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // Reference to your TextMesh Pro dropdown component.

    private static LanguageDropdownHandler instance;

    private string lastSelectedLanguageCode;

    /// <summary>
    /// Singleton instance to ensure only one LanguageDropdownHandler exists.
    /// </summary>
    public static LanguageDropdownHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LanguageDropdownHandler>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "LanguageManager";
                    instance = obj.AddComponent<LanguageDropdownHandler>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            UpdateDropdown();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        EventManager.OnLanguageChanged += UpdateDropdown;
    }

    private void OnDisable()
    {
        EventManager.OnLanguageChanged -= UpdateDropdown;
    }


    /// <summary>
    /// Updates the dropdown menu with available languages and sets the current language as the default selection.
    /// </summary>
    private void UpdateDropdown()
    {
        if (languageDropdown != null)
        {
            languageDropdown.ClearOptions();

            // Fetch the available language codes.
            var availableLanguageCodes = LanguageManager.Instance.GetAvailableLanguageCodes();

            List<string> translatedLanguageNames = new List<string>();

            // Fetch translated language names based on current selected language.
            foreach (var code in availableLanguageCodes)
            {
                string translationKey = $"lang_{code}";
                string translatedName = LocalizationManager.Instance.GetTranslation(translationKey);
                translatedLanguageNames.Add(translatedName);
            }

            languageDropdown.AddOptions(translatedLanguageNames);

            // Update the dropdown to show the name of languages in the currently selected language.
            string currentLanguageCode = GameManager.instance.languageCode;
            string currentLanguageTranslationKey = $"lang_{currentLanguageCode}";
            string currentTranslatedLanguageName = LocalizationManager.Instance.GetTranslation(currentLanguageTranslationKey);
            int currentIndex = translatedLanguageNames.IndexOf(currentTranslatedLanguageName);

            languageDropdown.value = currentIndex;
        }
    }

    /// <summary>
    /// Called when the value of the language dropdown changes.
    /// </summary>
    /// <param name="index">The index of the selected language in the dropdown.</param>
    public void OnLanguageDropdownValueChanged(int index)
    {
        // Get the selected language option from the dropdown.
        string selectedLanguage = languageDropdown.options[index].text;

        // Get the language code for the selected language.
        // Example: Nederlands (nl) -> "nl"
        string languageCode = selectedLanguage.Split(' ')[^1].Trim(new char[] { '(', ')' });
        lastSelectedLanguageCode = languageCode;


        // If the language has not changed, cancel.
        if (lastSelectedLanguageCode == GameManager.instance.languageCode)
        {
            Debug.Log($"Current language: {GameManager.instance.languageCode} is the same as the last selected language: {lastSelectedLanguageCode}");
            return;
        }

        EventManager.ButtonClicked(new CustomEventArgs(gameObject));

        // Set the selected language in the GameManager.
        GameManager.instance.SetLanguageCode(languageCode);
    }
}
