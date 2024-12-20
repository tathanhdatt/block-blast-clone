using UnityEngine;

[CreateAssetMenu(menuName = "Board Template")]
public class BoardTemplate : ScriptableObject
{
    public bool[] shape = new bool[GameConstant.boardSize * GameConstant.boardSize];
}