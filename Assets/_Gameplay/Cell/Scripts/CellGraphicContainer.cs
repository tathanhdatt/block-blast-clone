using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Board/Cell Graphic Container")]
public class CellGraphicContainer : ScriptableObject
{
    public List<CellGraphicItem> items;
}