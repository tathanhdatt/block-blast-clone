using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CellGraphicContainer))]
public class CellGraphicContainerEditor : Editor
{
    private CellGraphicContainer Container => target as CellGraphicContainer;

    public override void OnInspectorGUI()
    {
        if (Container == null) return;
        serializedObject.Update();
        AddButtons(Container);
        DrawItems(Container);
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(Container);
        }
    }

    private void DrawItems(CellGraphicContainer container)
    {
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

    private void AddButtons(CellGraphicContainer container)
    {
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
    }
}