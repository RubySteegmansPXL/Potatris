using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LanguageDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // Reference to your TextMesh Pro dropdown component.

    private void Start()
    {
        // Ensure the dropdown exists and is not null.
        if (languageDropdown != null)
        {
            // Clear any existing options from the dropdown.
            languageDropdown.ClearOptions();

            // Fetch the available language names from the LanguageManager.
            var availableLanguages = LanguageManager.Instance.GetAvailableLanguagesNames();

            // Populate the dropdown options with language names.
            languageDropdown.AddOptions(availableLanguages);
        }
    }
    public void OnLanguageDropdownValueChanged(int index)
    {
        // Get the selected language option from the dropdown.
        string selectedLanguage = languageDropdown.options[index].text;
        
        // Set the selected language in the GameManager.
        GameManager.instance.SetLanguageCode(selectedLanguage);
    }
}
