using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CellGraphicContainer))]
public class CellGraphicContainerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CellGraphicContainer container = target as CellGraphicContainer;
        if (container == null) return;
        GUILayout.BeginHorizontal();
        bool isClickAddButton = GUI.Button(new Rect(10, 10, 100, 30), "Add");
        bool isClickRemoveButton = GUI.Button(new Rect(120, 10, 100, 30), "Remove");
        GUILayout.EndHorizontal();
        EditorGUILayout.Space(50);
        if (isClickAddButton)
        {
            container.items.Add(new CellGraphicItem());
        }
        else if (isClickRemoveButton)
        {
            if (container.items.Count > 0)
            {
                container.items.RemoveAt(container.items.Count - 1);
            }
        }

        foreach (CellGraphicItem item in container.items)
        {
            EditorGUILayout.BeginHorizontal();
            item.id = (CellGraphicID)EditorGUILayout.EnumPopup(item.id);
            EditorGUILayout.Space(1);
            item.graphic = (Sprite)EditorGUILayout.ObjectField(item.graphic, typeof(Sprite), false);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5);
        }
    }
}