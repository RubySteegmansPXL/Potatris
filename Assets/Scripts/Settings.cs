using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/SettingsGenerator", order = 1)]
public class Settings : ScriptableObject
{
    [Header("Game Settings")]
    public bool isTutorial;
    public float defaultMoveSpeed = 1f;
    public float holdMoveSpeed = 0.1f;
    public float timeToHoldBeforeSpeedup = 0.3f;
    public ShapeData[] possibleShapes;
    public int maximumNudgeTries = 5; // How many times the shape can be attempted to nudge before placement is cancelled

    [HorizontalLine(color: EColor.Blue)]
    [Header("Board Settings")]
    [Range(0, 3)]
    public int numberOfShapesInQueue = 3;
    public int numberOfRows = 20;
    public int numberOfColumns = 11;
    public float lineClearDelay = 0.1f;
    [HorizontalLine(color: EColor.Blue)]

    [Header("Feedback Settings")]

    public string[] lineClearMessages = new string[] { "Nice!", "Great!", "Awesome!", "Amazing!" };
    public string[] tetrisMessages = new string[] { "UNBELIEVABLE!", "FANTASTIC!", "BINGOOO!", "WOW!" };
    public float popupMessageDuration = 1.5f;
    public float tetrisPopupMessageDuration = 2.5f;

    public Color32[] lineClearColors = new Color32[]
    {
        new Color32(255, 204, 204, 255),
        new Color32(204, 255, 204, 255),
        new Color32(204, 204, 255, 255)
    };

    public AnimationCurve lineClearTextAnimationCurve = AnimationCurve.Linear(0, 0, 1, 1);

}
