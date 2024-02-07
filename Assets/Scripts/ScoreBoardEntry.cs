using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class ScoreBoardEntry : MonoBehaviour
{
    public TextMeshProUGUI[] rankTexts;
    public TextMeshProUGUI[] nameTexts;
    public TextMeshProUGUI[] scoreTexts;

    public void SetEntry(int rank, string name, int score)
    {
        foreach (var rankText in rankTexts)
        {
            rankText.text = rank.ToString() + ".";
        }

        foreach (var nameText in nameTexts)
        {
            nameText.text = name;
        }

        foreach (var scoreText in scoreTexts)
        {
            scoreText.text = score.ToString("N0", CultureInfo.InvariantCulture);
        }
    }
}
