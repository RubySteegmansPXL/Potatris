using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScoreLocalizedText : LocalizedText
{
    public string key = "gameover_final_score";

    override public void UpdateText()
    {
        Scoring scoring = FindObjectOfType<Scoring>();
        int score = 0;
        if (scoring != null)
        {
            score = scoring.Score;
        }

        Debug.Log("FinalScoreLocalizedText.UpdateText: " + key + " " + score);
        textMeshPro.text = LocalizationManager.Instance.GetTranslation(key) + " " + (score == 0 ? "UNKNOWN" : score.ToString());
    }
}
