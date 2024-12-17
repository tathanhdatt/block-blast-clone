using UnityEditor;
using UnityEngine;

namespace Dt.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(TitleAttribute))]
    public class TitleDrawer : DecoratorDrawer
    {
        private static Rect rect = new Rect(0, 26, 1.3f, 1.3f);
        private const float AttributeHeight = 32;
        private static readonly Color LineColor = new Color(0.8f, 0.8f, 0.8f);

        private readonly GUIStyle style = new GUIStyle()
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white },
        };

        private TitleAttribute Attribute => attribute as TitleAttribute;

        public override void OnGUI(Rect position)
        {
            EditorGUI.LabelField(position, Attribute.Title, this.style);
            GUILayout.Space(30);
            rect.x = position.x;
            rect.width = position.width;
            EditorGUI.DrawRect(rect, LineColor);
        }

        public override float GetHeight()
        {
            return AttributeHeight;
        }
    }
}