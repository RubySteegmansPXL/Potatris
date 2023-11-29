using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Manages localization and loads translations from a CSV file.
/// </summary>
public class LocalizationManager : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, string>> translations;
    private string csvFilePath = "Assets/Localization/translations.csv";

    private static LocalizationManager instance;

    public static LocalizationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LocalizationManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "LocalizationManager";
                    instance = obj.AddComponent<LocalizationManager>();
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
            LoadTranslations();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Loads translations from a CSV file and populates the translations dictionary.
    /// </summary>
    public void LoadTranslations()
    {
        Debug.Log("Loading translations");
        // Load translations from the CSV file
        translations = new Dictionary<string, Dictionary<string, string>>();

        using (StreamReader file = new StreamReader(csvFilePath))
        {
            bool firstLine = true;

            while (!file.EndOfStream)
            {
                string line = file.ReadLine();
                Debug.Log("Line: " + line);

                if (firstLine)
                {
                    firstLine = false;
                    continue; // Skip the header row
                }

                // Use a custom CSV parsing method to handle quoted strings
                var parts = ParseCSVLine(line);
                Debug.Log("Parts: " + parts.Length);

                if (parts.Length >= 3)
                {
                    string key = parts[0].Trim();
                    string nlTranslation = parts[1].Trim();
                    string enTranslation = parts[2].Trim();
                    // Add more languages as needed.
                    Debug.Log("Key: " + key + " NL: " + nlTranslation + " EN: " + enTranslation);
                    
                    translations[key] = new Dictionary<string, string>
                    {
                        { "nl", nlTranslation },
                        { "en", enTranslation }
                    };
                }
            }
        }
    }

    /// <summary>
    /// Parses a line of CSV
    /// </summary>
   private string[] ParseCSVLine(string line)
    {
        List<string> parts = new List<string>();
        StringBuilder currentPart = new StringBuilder();
        bool insideQuotes = false;

        foreach (char c in line)
        {
            if (c == ';')
            {
                if (!insideQuotes)
                {
                    parts.Add(currentPart.ToString());
                    currentPart.Clear();
                    continue;
                }
            }
            else if (c == '"')
            {
                insideQuotes = !insideQuotes;
                continue;
            }

            currentPart.Append(c);
        }

        parts.Add(currentPart.ToString());
        return parts.ToArray();
    }

    /// <summary>
    /// Retrieves a translation for a given key and current language.
    /// </summary>
    /// <returns>The translated text or "Translation not found" if not found.</returns>
    public string GetTranslation(string key)
        {
            string currentLanguageCode = GameManager.instance.languageCode; 
            if (translations.ContainsKey(key) && translations[key].ContainsKey(currentLanguageCode))
            {
                return translations[key][currentLanguageCode];
            }
            else
            {
                return "Translation not found";
            }
        }
}
