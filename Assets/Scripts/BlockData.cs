using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Tetris/Block Data")]
public class BlockData : ScriptableObject
{
    public Color baseColor;
    public Color accentColor;
    public Color lightColor;
}