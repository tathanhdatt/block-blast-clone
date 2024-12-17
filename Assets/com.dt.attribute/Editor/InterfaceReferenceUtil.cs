using UnityEditor;
using UnityEngine;

namespace Dt.Attribute.Editor
{
    public static class InterfaceReferenceUtil
    {
        private static GUIStyle labelStyle;

        public static void OnGUI(Rect position, InterfaceArgs args, Object assignedObject)
        {
            InitializeStyleIfNeeded();
            int controlID = GUIUtility.GetControlID(FocusType.Passive) - 1;
            bool isHover = position.Contains(Event.current.mousePosition);
            string displayName;
            if (isHover || assignedObject == null)
            {
                displayName = args.InterfaceType.Name;
            }
            else
            {
                displayName = "*";
            }
            DrawInterfaceNameLabel(position, displayName, controlID);
        }

        private static void DrawInterfaceNameLabel(Rect position, string displayName, int controlID)
        {
            if (Event.current.type != EventType.Repaint) return;
            const int additionalLeftWidth = 3;
            const int verticalIndent = 1;

            GUIContent content = EditorGUIUtility.TrTextContent(displayName);
            Vector2 size = labelStyle.CalcSize(content);
            Rect labelPosition = position;

            labelPosition.width = size.x + additionalLeftWidth;
            labelPosition.x += position.width - labelPosition.width - 18;
            labelPosition.height -= verticalIndent * 2;
            labelPosition.y += verticalIndent;
            labelStyle.Draw(labelPosition, content, controlID,
                DragAndDrop.activeControlID == controlID, false);
        }

        private static void InitializeStyleIfNeeded()
        {
            if (labelStyle != null) return;

            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                fontStyle = EditorStyles.objectField.fontStyle,
                fontSize = EditorStyles.objectField.fontSize,
                font = EditorStyles.objectField.font,
                alignment = TextAnchor.MiddleRight,
                padding = new RectOffset(0, 2, 0, 0),
            };
            labelStyle = style;
        }
    }
}