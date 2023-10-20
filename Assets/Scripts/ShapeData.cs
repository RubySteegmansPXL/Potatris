using UnityEngine;

[CreateAssetMenu(fileName = "ShapeData", menuName = "Tetris/Shape Data")]
public class ShapeData : ScriptableObject
{
    public int x;
    public int y;
    public bool isCenter;
}