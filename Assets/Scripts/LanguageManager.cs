using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    private Dictionary<string, string> languageDictionary = new Dictionary<string, string>();

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

    private void InitializeLanguageDictionary()
    {
        // Add your language code and name pairs to the dictionary.
        languageDictionary["en"] = "English";
        languageDictionary["nl"] = "Dutch";
        // Add more languages as needed.

        // You can also load this data from a file or another source.
    }

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
