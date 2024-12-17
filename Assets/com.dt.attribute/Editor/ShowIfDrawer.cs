using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dt.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        private ShowIfAttribute ShowIfAttribute => attribute as ShowIfAttribute;
        private FieldInfo conditionFieldInfo;
        private MethodInfo conditionMethodInfo;
        private Object targetObject;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            this.targetObject = property.serializedObject.targetObject;
            GetConditionFieldInfo();

            if (this.conditionFieldInfo == null)
            {
                GetConditionMethodInfo();
                if (this.conditionMethodInfo == null)
                {
                    EditorGUI.EndProperty();
                    GUI.enabled = true;
                }
                else
                {
                    DrawIfMethodCondition(position, property, label);
                }

                return;
            }

            Type conditionType = this.conditionFieldInfo.FieldType;
            if (conditionType == typeof(bool))
            {
                DrawIfBoolCondition(position, property, label);
            }
            else if (conditionType.IsEnum)
            {
                DrawIfEnumCondition(position, property, label);
            }

            EditorGUI.EndProperty();
            GUI.enabled = true;
        }

        private void DrawIfMethodCondition(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (this.conditionMethodInfo.ReturnType != typeof(bool))
            {
                throw new InvalidOperationException("Return type must be bool!");
            }

            bool canShow = (bool)this.conditionMethodInfo.Invoke(this.targetObject, null);
            if (canShow)
            {
                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label);
                GUI.enabled = true;
            }
        }

        private void GetConditionMethodInfo()
        {
            this.conditionMethodInfo =
                this.targetObject.GetType().GetMethod(this.ShowIfAttribute.Condition,
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private void DrawIfBoolCondition(
            Rect position, SerializedProperty property, GUIContent label)
        {
            bool canShow = (bool)this.conditionFieldInfo.GetValue(this.targetObject);
            if (canShow)
            {
                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label);
                GUI.enabled = true;
            }
        }

        private void DrawIfEnumCondition(Rect position, SerializedProperty property,
            GUIContent label)
        {
            Type actualType = this.conditionFieldInfo.FieldType;
            Type expectedType = this.ShowIfAttribute.EnumValue.GetType();
            if (actualType != expectedType)
            {
                throw new ArgumentException(
                    $"{actualType} is not the expected type {expectedType}!");
            }

            object actualValue = this.conditionFieldInfo.GetValue(this.targetObject);
            object expectedValue = this.ShowIfAttribute.EnumValue;
            if (actualValue.Equals(expectedValue))
            {
                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label);
                GUI.enabled = true;
            }
        }

        private void GetConditionFieldInfo()
        {
            this.conditionFieldInfo = this.targetObject.GetType()
                .GetField(ShowIfAttribute.Condition,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }
    }
}