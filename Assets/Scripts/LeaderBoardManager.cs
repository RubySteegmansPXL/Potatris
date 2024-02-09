using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Dan.Main;

public class LeaderBoardManager : MonoBehaviour
{
    public ScoreBoardEntry scoreBoardEntryPrefab;
    public GameObject contentParent;

    private string publicLeaderboardKey = "5287a9b8517bb9be540e317f1d2f26ca0915770b3f69d92b2d8c916ccb37d253";
    //private string secretkey = "0709809a938241f55203cfc08e40e3726236a9bd56723b83cff693e3bad8100c71d80fc3774c3be146cd0b0a0726372827ef0b823e72c2baf174ef7cf29c44f668636a52c924b239411d85264652fa7152209009dbbf1e3f97c14cb6e17e7a932ccc97eddc226f82c3de3b525140306077bad67de764d47a9ca4db16ac80bd02";

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (leaderboard) =>
        {
            foreach (Transform child in contentParent.transform)
            {
                Destroy(child.gameObject);
            }

            int numberOfEntries = leaderboard.Length;
            if (numberOfEntries > 100)
            {
                numberOfEntries = 100;
            }

            for (int i = 0; i < numberOfEntries; i++)
            {
                ScoreBoardEntry entry = Instantiate(scoreBoardEntryPrefab, contentParent.transform);
                entry.SetEntry(i + 1, leaderboard[i].Username, leaderboard[i].Score);
            }
        });
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.ResetPlayer();
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, (leaderboard) =>
        {
            GetLeaderboard();
        });
    }

    private void Start()
    {
        GetLeaderboard();
    }

    public static void AddEntry(string username, int score)
    {
        LeaderboardCreator.ResetPlayer();
        LeaderboardCreator.UploadNewEntry("5287a9b8517bb9be540e317f1d2f26ca0915770b3f69d92b2d8c916ccb37d253", username, score, (leaderboard) =>
        {
            Debug.Log("Leaderboard updated");
        });
    }
}
