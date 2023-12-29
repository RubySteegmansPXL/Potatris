using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Achievement {
    public string id;
    public string titleKey;
    public string descKey;
    public bool isUnlocked;
    public int  progress;
    public int goal;
}
