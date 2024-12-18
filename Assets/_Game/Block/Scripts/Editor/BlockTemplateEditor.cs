using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BlockTemplate))]
public class BlockTemplateEditor : Editor
{
    private BlockTemplate Template => target as BlockTemplate;
    private GUIStyle toggleStyle;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawFields();
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(Template);
        }
    }

    private void DrawFields()
    {
        this.toggleStyle = CreateToggleStyle();
        int lastWidth = Template.width;
        int lastHeight = Template.height;
        DrawSizeInput();
        CreateTemplateShapeIfChangeSize(lastWidth, lastHeight);
        GUILayout.Space(10);
        GUILayout.BeginVertical();
        DrawShapeInput();
        GUILayout.EndVertical();
    }

    private void DrawShapeInput()
    {
        for (int i = 0; i < Template.height; i++)
        {
            GUILayout.BeginHorizontal();
            DrawShapeRowInput(i);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }
    }

    private void DrawShapeRowInput(int columnIndex)
    {
        for (int j = 0; j < Template.width; j++)
        {
            int index = columnIndex * Template.width + j;
            Template.shape[index] = GUILayout.Toggle(Template.shape[index], "", this.toggleStyle);
            GUILayout.Space(10);
        }
    }

    private void CreateTemplateShapeIfChangeSize(int lastWidth, int lastHeight)
    {
        bool isChangedWidth = lastWidth != Template.width;
        bool isChangedHeight = lastHeight != Template.height;
        if (isChangedWidth || isChangedHeight)
        {
            Template.shape = new bool[Template.width * Template.height];
        }
    }

    private void DrawSizeInput()
    {
        GUILayout.BeginVertical();
        Template.width = EditorGUILayout.IntField("Width", Template.width);
        Template.height = EditorGUILayout.IntField("Height", Template.height);
        GUILayout.EndVertical();
    }

    private GUIStyle CreateToggleStyle()
    {
        GUIStyle toggleStyle = new GUIStyle(EditorStyles.miniButtonMid)
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
        return toggleStyle;
    }
}