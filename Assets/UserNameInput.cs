using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UserNameInput : MonoBehaviour
{
    private TMP_InputField inputField;
    public Button submitButton;
    public GameObject otherButtons;

    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        submitButton.onClick.AddListener(SubmitName);
        // on edit
        inputField.onValueChanged.AddListener(PlayEditSound);
    }

    private void PlayEditSound(string input)
    {
        EventManager.ButtonHover(new CustomEventArgs(gameObject));
    }

    // Submit the name to the leaderboard (uppercase only)
    private void SubmitName()
    {
        Scoring scoring = FindObjectOfType<Scoring>();
        if (scoring == null)
        {
            Debug.LogError("Scoring not found");
            gameObject.SetActive(false);
        }
        LeaderBoardManager.AddEntry(inputField.text, scoring.Score);

        Debug.Log("Entry submitted: " + inputField.text.ToUpper() + " - " + scoring.Score);

        otherButtons.SetActive(true);
        gameObject.SetActive(false);
    }


}
