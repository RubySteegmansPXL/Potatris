using UnityEngine;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages the dropdown menu for language selection.
/// </summary>
public class LanguageDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // Reference to your TextMesh Pro dropdown component.

    private static LanguageDropdownHandler instance;

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


    /// <summary>
    /// Updates the dropdown menu with available languages and sets the current language as the default selection.
    /// </summary>
    private void UpdateDropdown() 
    {
        // Ensure the dropdown exists and is not null.
        if (languageDropdown != null)
        {
            // Clear the dropdown options.
            languageDropdown.ClearOptions();

            // Fetch the available language names from the LanguageManager.
            var availableLanguages = LanguageManager.Instance.GetAvailableLanguagesNames();

            // Populate the dropdown options with language names.
            languageDropdown.AddOptions(availableLanguages);

            // Get the index of the current language in the available languages list.
            string currentLanguage = LanguageManager.Instance.GetLanguageName(GameManager.instance.languageCode);
            int currentIndex = availableLanguages.IndexOf(currentLanguage);

            // Set the dropdown's value to the index of the current language.
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
        
        // Set the selected language in the GameManager.
        GameManager.instance.SetLanguageCode(selectedLanguage);
    }
}
