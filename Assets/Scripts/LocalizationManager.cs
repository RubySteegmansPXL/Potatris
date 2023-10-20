using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, string>> translations;
    public string currentLanguage = "en"; // Default language
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

    

    public void SetLanguage(string languageCode)
    {
        if (translations.ContainsKey(languageCode))
        {
            currentLanguage = languageCode;
        }
        else
        {
            currentLanguage = "en"; // Fallback to English if the requested language is not found.
        }
    }

    public void LoadTranslations()
    {
        // Load translations from the CSV file
        translations = new Dictionary<string, Dictionary<string, string>>();

        using (StreamReader file = new StreamReader(csvFilePath))
        {
            bool firstLine = true;

            while (!file.EndOfStream)
            {
                string line = file.ReadLine();

                if (firstLine)
                {
                    firstLine = false;
                    continue; // Skip the header row
                }

                // Use a custom CSV parsing method to handle quoted strings
                var parts = ParseCSVLine(line);

                if (parts.Length >= 4)
                {
                    string key = parts[0].Trim();
                    string nlTranslation = parts[2].Trim();
                    string enTranslation = parts[3].Trim();
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

    private string[] ParseCSVLine(string line)
    {
        List<string> parts = new List<string>();
        StringBuilder currentPart = new StringBuilder();
        bool insideQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                insideQuotes = !insideQuotes;
            }
            else if (c == ',' && !insideQuotes)
            {
                parts.Add(currentPart.ToString());
                currentPart.Clear();
            }
            else
            {
                currentPart.Append(c);
            }
        }

        parts.Add(currentPart.ToString());

        return parts.ToArray();
    } 

    public string GetTranslation(string key)
        {
            if (translations.ContainsKey(key) && translations[key].ContainsKey(currentLanguage))
            {
                return translations[key][currentLanguage];
            }
            else
            {
                return "Translation not found";
            }
        }
}
