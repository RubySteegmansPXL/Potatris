// using System.Collections.Generic;
// using UnityEngine;
// public class AchievementsManager : MonoBehaviour {
    
    
//     public List<GameObject> achievementGameObjects; // List of achievement gameObjects in the scene
//     public GameObject achievementPrefab;   // Prefab for achievement gameObject
//     private List<Achievement> achievements; // List of achievements

//     // Method to load achievements from parsed CSV data
//     void LoadAchievements() {
//         // Load achievement data from CSV (pseudocode)
//         List<Achievement> achievements = LoadAchievementDataFromCSV();

//         // Assume you have a way to track completion percentage for each achievement
//         // For demo, assigning random completion percentages
//         foreach (Achievement achievement in achievements) {
//             achievement.completionPercentage = Random.Range(0f, 100f);
//         }

//         // Display achievements in UI
//         DisplayAchievements(achievements);
//     }

//     // Method to display achievements in UI
//     void DisplayAchievements(List<Achievement> achievements) {
//         // For demonstration, displaying the first achievement
//         Achievement firstAchievement = achievements[0];
//         titleText.text = firstAchievement.title;
//         descriptionText.text = firstAchievement.description;
//         completionPercentageText.text = $"{firstAchievement.completionPercentage}%";
//     }

//     // Call this method to initiate loading and displaying achievements
//     void Start() {
//         LoadAchievements();
//     }
// }
