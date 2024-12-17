using UnityEditor;
using UnityEngine;

namespace Dt.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelDrawer : PropertyDrawer
    {
        private LabelAttribute Attribute => attribute as LabelAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            label.text = Attribute.Label;
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndProperty();
        }
    }
}