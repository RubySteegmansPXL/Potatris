using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUI : MonoBehaviour
{
    public List<TextMeshProUGUI> titleTexts;
    public TextMeshProUGUI descText;
    public Image unlockImage;
    public TextMeshProUGUI progressText;


    public void SetAchievement(Achievement achievement, bool isPopUp)
    {
        if (!isPopUp)
        {
            foreach (var titleText in titleTexts)
            {
                titleText.text = LocalizationManager.Instance.GetTranslation(achievement.titleKey);
            }
            descText.text = LocalizationManager.Instance.GetTranslation(achievement.descKey);
            unlockImage.color = achievement.isUnlocked ? Color.green : Color.red;
            progressText.text = $"{achievement.progress}%";
        } else
        {
            foreach (var titleText in titleTexts)
            {
                titleText.text = LocalizationManager.Instance.GetTranslation(achievement.titleKey);
            }
            descText.text = LocalizationManager.Instance.GetTranslation(achievement.descKey);
        }

    }
}
