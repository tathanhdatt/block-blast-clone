using UnityEditor;
using UnityEngine;

namespace Dt.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label);
            bool isReferenceType = property.propertyType == SerializedPropertyType.ObjectReference;
            if (!isReferenceType) return;
            bool hasValue = property.objectReferenceValue != null;
            if (hasValue) return;
            GUILayout.Space(30);
            Rect warningRect = new Rect(
                position.x, position.y + position.height * 1.3f,
                position.width, position.height * 1.3f);
            EditorGUI.HelpBox(warningRect, "Required field is missing.", MessageType.Error);
        }
    }
}