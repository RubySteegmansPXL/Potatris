using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementsManager : MonoBehaviour
{

    public static AchievementsManager instance;
    public List<Achievement> achievements;
    public GameObject achievementPrefab;
    public GameObject achievementPopUpPrefab;
    private Transform scrollViewContent;
    private Transform popUpParent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAchievements();
            if (achievements.Count == 0)
            {
                InstantiateAchievements();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();

        if (GameManager.instance.settings.isTutorial)
        {
            return;
        }

        Debug.Log("ACHIEVEMENTS" + scene.buildIndex);
        if (scene.buildIndex == 3)
        {
            Debug.Log("OnSceneLoaded Achievements");
            scrollViewContent = GameObject.FindGameObjectWithTag("ScrollViewContent").transform;
            Debug.Log(scrollViewContent);
            PopulateScrollView();
        }
    }

    private void InstantiateAchievements()
    {
        for (int i = 1; i <= 10; i++)
        {
            Achievement achievement = new Achievement()
            {
                id = $"ach_{i}",
                titleKey = $"ach_title_{i}",
                descKey = $"ach_desc_{i}",
                isUnlocked = false,
                progress = 0,
                goal = 100
            };

            achievements.Add(achievement);
        }

        achievements[8].goal = 5000;
        achievements[9].goal = 20000;
        SaveAchievements();
    }

    public void UpdateAchievement(string id, int progressToAdd)
    {

        if (GameManager.instance.settings.isTutorial)
        {
            return;
        }

        Achievement achievement = achievements.Find(a => a.id == id);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.progress += progressToAdd;
            Debug.Log("progress and goal" + achievement.progress + achievement.goal);
            if (achievement.progress >= achievement.goal)
            {
                achievement.isUnlocked = true;
                UnlockAchievement(achievement.id);
            }

            SaveAchievements();
        }
    }

    public void PopulateScrollView()
    {
        // Clear the scroll view content
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
            Debug.Log("Destroyed a child");
        }
        Debug.Log("Trying to populate scroll view content");
        Debug.Log(achievements.Count);

        // Populate the scroll view content with achievements
        foreach (Achievement achievement in achievements)
        {
            Debug.Log(achievement.titleKey);
            GameObject achievementObject = Instantiate(achievementPrefab);
            achievementObject.transform.SetParent(scrollViewContent, false);

            AchievementUI achievementUI = achievementObject.GetComponent<AchievementUI>();
            achievementUI.SetAchievement(achievement, false);
        }
    }

    public void UnlockAchievement(string id)
    {
        Debug.Log("UnlockAchievement" + id);
        Achievement achievement = achievements.Find(a => a.id == id);
        Debug.Log(achievement.titleKey);
        popUpParent = GameObject.FindGameObjectWithTag("AchievementPopUp").transform;
        GameObject achievementPopUp = Instantiate(achievementPopUpPrefab);
        AchievementUI achievementUI = achievementPopUp.GetComponent<AchievementUI>();
        achievementUI.SetAchievement(achievement, true);
        achievementPopUp.transform.SetParent(popUpParent, false);
        EventManager.AchievementUnlocked(new CustomEventArgs(gameObject));
        StartCoroutine(DestroyAchievement(achievementPopUp));
    }

    public void SaveAchievements()
    {
        AchievementList achievementList = new AchievementList(achievements);
        string json = JsonUtility.ToJson(achievementList);
        PlayerPrefs.SetString("Achievements", json);
    }

    public void LoadAchievements()
    {
        string json = PlayerPrefs.GetString("Achievements");
        if (!string.IsNullOrEmpty(json))
        {
            Debug.Log("LoadAchievements json not null");
            AchievementList achievementList = JsonUtility.FromJson<AchievementList>(json);
            achievements = achievementList.achievements;
        }
        else
        {
            Debug.Log("LoadAchievements json null");
            achievements = new List<Achievement>();
        }
    }

    public string GetLocalizedTitle(Achievement achievement)
    {
        return LocalizationManager.Instance.GetTranslation(achievement.titleKey);
    }

    public string GetLocalizedDescription(Achievement achievement)
    {
        return LocalizationManager.Instance.GetTranslation(achievement.descKey);
    }

    IEnumerator DestroyAchievement(GameObject popUp)
    {
        float counter = 0;
        Vector3 originalPosition = popUp.transform.position;
        popUp.transform.position = new Vector3(popUp.transform.position.x + 500, popUp.transform.position.y, popUp.transform.position.z);
        while (counter < 1)
        {
            counter += Time.deltaTime;
            popUp.transform.position = Vector3.Lerp(popUp.transform.position, originalPosition, 0.1f);
            yield return null;
        }
        yield return new WaitForSeconds(3);
        //Move popUp to the right
        counter = 0;
        while (counter < 1)
        {
            counter += Time.deltaTime;
            popUp.transform.position = Vector3.Lerp(popUp.transform.position, new Vector3(popUp.transform.position.x + 5, popUp.transform.position.y, popUp.transform.position.z), 0.9f);
            yield return null;
        }
        //Destroy popUp
        Destroy(popUp);
    }

}

[Serializable]
public class AchievementList
{
    public List<Achievement> achievements;

    public AchievementList(List<Achievement> achievements)
    {
        this.achievements = achievements;
    }
}