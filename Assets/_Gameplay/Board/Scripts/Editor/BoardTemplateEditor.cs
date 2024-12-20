using Dt.Extension;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoardTemplate))]
public class BoardTemplateEditor : Editor
{
    private BoardTemplate Template => target as BoardTemplate;
    private GUIStyle toggleStyle;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        this.toggleStyle = CreateToggleStyle();
        if (GUILayout.Button("Clear All"))
        {
            Template.shape.Clear();
        }
        GUILayout.Space(10);
        for (int i = GameConstant.boardSize - 1; i >= 0; i--)
        {
            GUILayout.BeginHorizontal();
            DrawShapeRowInput(i);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(Template);
        }
    }

    private void DrawShapeRowInput(int rowIndex)
    {
        for (int j = 0; j < GameConstant.boardSize; j++)
        {
            int index = rowIndex * GameConstant.boardSize + j;
            Template.shape[index] = GUILayout.Toggle(Template.shape[index],
                $"{rowIndex}|{j}", this.toggleStyle);
            GUILayout.Space(10);
        }
    }

    private GUIStyle CreateToggleStyle()
    {
        GUIStyle style = new GUIStyle(EditorStyles.miniButtonMid)
        {
            fixedWidth = 50,
            fixedHeight = 50,
            normal = new GUIStyleState
            {
                background = Texture2D.linearGrayTexture
            },
            onNormal = new GUIStyleState
            {
                background = Texture2D.whiteTexture
            }
        };
        return style;
    }
}