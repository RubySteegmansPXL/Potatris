using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;

/// <summary>
/// Manages localization and loads translations from a CSV file.
/// </summary>
public class LocalizationManager : MonoBehaviour
{
    private Dictionary<string, Dictionary<string, string>> translations;
    private string csvFilePath = "Localization/translations.csv";

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
            Debug.Log("LocalizationManager instance created");
            instance = this;
            DontDestroyOnLoad(gameObject);

            Debug.Log("Loading translations");
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
        // Initialize the translations dictionary
        translations = new Dictionary<string, Dictionary<string, string>>();

        // The path to the file in the Resources folder, without the file extension
        string resourcePath = "Localization/translations";

        try
        {
            // Load the CSV file from the Resources folder
            TextAsset csvData = Resources.Load<TextAsset>(resourcePath);
            if (csvData != null)
            {
                // Split the file content by new lines to get each line of the CSV
                string[] lines = csvData.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                bool firstLine = true;

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

                    if (firstLine)
                    {
                        firstLine = false;
                        // Send the header to the initializer
                        LanguageManager.Instance.InitializeLanguageDictionary(line);
                        continue; // Skip the header row
                    }

                    var parts = ParseCSVLine(line);

                    if (parts.Length >= 3)
                    {
                        string key = parts[0].Trim();

                        string[] allLanguageTranslations = parts.Skip(1).Select(x => x.Trim()).ToArray();
                        string[] allLanguageCodes = LanguageManager.Instance.GetAvailableLanguageCodes().ToArray();

                        for (int i = 0; i < allLanguageTranslations.Length; i++)
                        {
                            if (!translations.ContainsKey(key))
                            {
                                translations.Add(key, new Dictionary<string, string>());
                            }
                            translations[key][allLanguageCodes[i]] = allLanguageTranslations[i];
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Localization CSV data not found at path: " + resourcePath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading translations: " + e.Message);
        }

    }

    /// <summary>
    /// Parses a line of CSV data considering quoted strings and escaped double quotes.
    /// </summary>
    /// <param name="line">The line of CSV data to parse.</param>
    /// <returns>An array containing the parsed segments.</returns>
    private string[] ParseCSVLine(string line)
    {
        List<string> parts = new List<string>(); // List to store parsed segments
        StringBuilder currentPart = new StringBuilder(); // String builder to construct each segment
        bool insideQuotes = false; // Flag to track if currently inside quoted string

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // Check for two consecutive double quotes to interpret as a single double quote within a string
                if (i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentPart.Append('"'); // Add a single double quote to the segment
                    i++; // Skip the next quote
                    continue;
                }

                insideQuotes = !insideQuotes; // Toggle insideQuotes flag when encountering a double quote
            }
            else if (c == ';' && !insideQuotes)
            {
                parts.Add(currentPart.ToString()); // Add the completed segment to the list
                currentPart.Clear(); // Clear the StringBuilder for the next segment
                continue;
            }

            currentPart.Append(c); // Append the character to the current segment being built
        }

        parts.Add(currentPart.ToString()); // Add the last segment after the loop ends

        return parts.ToArray(); // Convert the list of segments to an array and return
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
