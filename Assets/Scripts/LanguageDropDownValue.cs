using UnityEngine;
using TMPro;

public class LanguageDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // Reference to your TextMesh Pro dropdown component.

    public void OnLanguageDropdownValueChanged(int index)
    {
        // Get the selected language option from the dropdown.
        string selectedLanguage = languageDropdown.options[index].text;
        
        // Set the selected language in the GameManager.
        GameManager.instance.SetLanguage(selectedLanguage.ToUpper());
    }
}
