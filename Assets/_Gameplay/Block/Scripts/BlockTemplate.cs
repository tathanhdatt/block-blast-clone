using UnityEngine;

[CreateAssetMenu(fileName = "Block Template", menuName = "Block Template")]
public class BlockTemplate : ScriptableObject
{
    public int width;
    public int height;
    public bool[] shape;
    public BlockType type;
}