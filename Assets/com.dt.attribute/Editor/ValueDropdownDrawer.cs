using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Dt.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(ValueDropdownAttribute))]
    public class ValueDropdownDrawer : PropertyDrawer
    {
        private ValueDropdownAttribute Attribute =>
            attribute as ValueDropdownAttribute;

        private object source;
        private FieldInfo fieldValues;
        private MethodInfo methodValues;

        private int lastSelected;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            GetSource(property);
            GetFieldValues();
            if (this.fieldValues != null)
            {
                HandleContentsFromField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            GetMethodValues();
            if (this.methodValues != null)
            {
                HandleContentsFromMethod(position, property, label);
            }
        }

        private void HandleContentsFromMethod(Rect position, SerializedProperty property,
            GUIContent label)
        {
            try
            {
                if (this.methodValues.Invoke(this.source, null) is not IList<string> values)
                {
                    throw new InvalidOperationException(
                        $"{this.fieldValues.FieldType} must be {typeof(IList<string>)}.");
                }

                if (values.Count == 0)
                {
                    WarningNoValues(position, property, label);
                }
                else
                {
                    DisplayContentAndSetPropertyValue(position, property, label, values);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void GetMethodValues()
        {
            this.methodValues = this.source.GetType().GetMethod(Attribute.Values,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        private void HandleContentsFromField(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (this.fieldValues.GetValue(this.source) is not IList<string> values)
            {
                throw new InvalidOperationException(
                    $"{this.fieldValues.FieldType} must be {typeof(IList<string>)}.");
            }

            if (values.Count == 0)
            {
                WarningNoValues(position, property, label);
                return;
            }

            DisplayContentAndSetPropertyValue(position, property, label, values);
        }

        private void WarningNoValues(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
            GUILayout.Space(28);
            Rect warningRect = new Rect(
                position.x, position.y + position.height * 1.3f,
                position.width, position.height * 1.3f);
            EditorGUI.HelpBox(warningRect, "No values provided.", MessageType.Warning);
        }

        private void DisplayContentAndSetPropertyValue(Rect position,
            SerializedProperty property, GUIContent label, IList<string> values)
        {
            GUIContent[] contents = GetContents(values);
            this.lastSelected = GetLastSelectedIfHave(values, property);
            this.lastSelected = DisplayContent(position, label, contents);
            SetPropertyValue(property, values);
        }

        private GUIContent[] GetContents(IList<string> values)
        {
            GUIContent[] content = new GUIContent[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                content[i] = new GUIContent(values[i]);
            }

            return content;
        }

        private int GetLastSelectedIfHave(IList<string> values, SerializedProperty property)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (property.stringValue == values[i])
                {
                    return i;
                }
            }

            return 0;
        }

        private void SetPropertyValue(SerializedProperty property, IList<string> values)
        {
            property.stringValue = values[this.lastSelected];
        }

        private int DisplayContent(Rect position, GUIContent label, GUIContent[] contents)
        {
            return EditorGUI.Popup(position, label, this.lastSelected, contents);
        }

        private void GetFieldValues()
        {
            this.fieldValues = this.source.GetType().GetField(Attribute.Values,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        private void GetSource(SerializedProperty property)
        {
            this.source = property.serializedObject.targetObject;
        }
    }
}