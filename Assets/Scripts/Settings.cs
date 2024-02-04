using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/SettingsGenerator", order = 1)]
public class Settings : ScriptableObject
{
    [Header("Shape Settings")]
    public float defaultMoveSpeed = 1f;
    public float holdMoveSpeed = 0.1f;
    public float timeToHoldBeforeSpeedup = 0.3f;
    public ShapeData[] possibleShapes;
    public int maximumNudgeTries = 5; // How many times the shape can be attempted to nudge before placement is cancelled

    [Header("Game Settings")]
    [Range(0, 3)]
    public int numberOfShapesInQueue = 3;
    public int numberOfRows = 20;
    public int numberOfColumns = 11;
    public float lineClearDelay = 0.1f;

}
