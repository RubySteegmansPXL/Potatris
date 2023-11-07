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

            // Initialize the language dictionary.
            InitializeLanguageDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initializes the language dictionary with language code and name pairs.
    /// </summary>
    private void InitializeLanguageDictionary()
    {
        languageDictionary["en"] = "English";
        languageDictionary["nl"] = "Dutch";
        // Add more languages as needed.
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
}
