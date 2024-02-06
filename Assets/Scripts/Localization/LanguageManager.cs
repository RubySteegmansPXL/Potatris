using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages supported languages and their codes.
/// </summary>
public class LanguageManager : MonoBehaviour
{
    private Dictionary<string, string> languageDictionary = new Dictionary<string, string>(); // Stores language codes and their corresponding names.

    private static LanguageManager instance;

    public static LanguageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LanguageManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "LanguageManager";
                    instance = obj.AddComponent<LanguageManager>();
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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initializes the language dictionary with language code and name pairs.
    /// Gets called from LocalizationManager
    /// Parses the first line from the CSV file to add languages to the language dictionary.
    /// </summary>
    public void InitializeLanguageDictionary(string firstLine)
    {

        // Split the line into tokens
        string[] tokens = firstLine.Split(';');
        // First token can be discarded, this is "Key"

        for (int i = 1; i < tokens.Length; i++)
        {
            // This will give us back something like "English(en)" or "Dutch(nl)"
            string[] languageTokens = tokens[i].Split('(');
            // We need to remove the last character from the language code, which is the closing parenthesis
            string languageCode = languageTokens[1].Substring(0, languageTokens[1].Length - 1);
            // Add the language code and name to the dictionary
            languageDictionary.Add(languageCode, languageTokens[0]);
        }
    }

    /// <summary>
    /// Retrieves the language name for a given language code.
    /// </summary>
    /// <param name="languageCode">The language code (e.g., "en").</param>
    /// <returns>The language name or "Unknown Language" if not found.</returns>
    public string GetLanguageName(string languageCode)
    {
        if (languageDictionary.ContainsKey(languageCode))
        {
            return languageDictionary[languageCode];
        }
        else
        {
            return "Unknown Language";
        }
    }

    /// <summary>
    /// Retrieves the language code for a given language name.
    /// </summary>
    /// <param name="languageName">The language name (e.g., "English").</param>
    /// <returns>The language code or "Unknown Language" if not found.</returns>
    public string GetLanguageCode(string languageName)
    {
        foreach (var pair in languageDictionary)
        {
            if (pair.Value.Equals(languageName, StringComparison.OrdinalIgnoreCase))
            {
                return pair.Key;
            }
        }
        return "Unknown Language";
    }

    /// <summary>
    /// Retrieves a list of available languages.
    /// </summary>
    /// <returns>A list of available language names.</returns>
    public List<string> GetAvailableLanguagesNames()
    {
        List<string> availableLanguages = new List<string>();

        foreach (var languageName in languageDictionary.Values)
        {
            availableLanguages.Add(languageName);
        }

        return availableLanguages;
    }

    public List<string> GetAvailableLanguageCodes()
    {
        return new List<string>(languageDictionary.Keys);
    }
}
