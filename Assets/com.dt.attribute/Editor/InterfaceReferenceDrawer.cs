using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dt.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceReference<>))]
    [CustomPropertyDrawer(typeof(InterfaceReference<,>))]
    public class InterfaceReferenceDrawer : PropertyDrawer
    {
        private const string UnderlyingValueFieldName = "underlyingValue";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty underlyingProperty =
                property.FindPropertyRelative(UnderlyingValueFieldName);
            InterfaceArgs args = GetInterfaceArgs(fieldInfo);

            EditorGUI.BeginProperty(position, label, underlyingProperty);
            Object assignedObject = EditorGUI.ObjectField(position, label,
                underlyingProperty.objectReferenceValue, typeof(Object), true);
            if (assignedObject != null)
            {
                if (assignedObject is GameObject gameObject)
                {
                    ValidateAndAssignObject(underlyingProperty,
                        gameObject.GetComponent(args.InterfaceType),
                        gameObject.name,
                        args.InterfaceType.Name);
                }
                else
                {
                    ValidateAndAssignObject(underlyingProperty, assignedObject,
                        args.InterfaceType.Name);
                }
            }
            else
            {
                underlyingProperty.objectReferenceValue = null;
            }

            EditorGUI.EndProperty();
            InterfaceReferenceUtil.OnGUI(position, args, assignedObject);
        }

        private static void ValidateAndAssignObject(SerializedProperty property,
            Object targetObject,
            string componentNameOrType, string interfaceName = null)
        {
            if (targetObject != null)
            {
                property.objectReferenceValue = targetObject;
            }
            else
            {
                string warningMessage = interfaceName != null
                    ? $"GameObject '{componentNameOrType}'"
                    : $"Assigned object";
                Debug.LogWarning(
                    $"{warningMessage} does not have a component that implements '{componentNameOrType}'");
                property.objectReferenceValue = null;
            }
        }

        private static InterfaceArgs GetInterfaceArgs(FieldInfo fieldInfo)
        {
            Type fieldType = fieldInfo.FieldType;

            if (!TryGetInterfaceArgs(fieldType, out Type objectType, out Type interfaceType))
            {
                GetTypeFromList(fieldType, out objectType, out interfaceType);
            }

            return new InterfaceArgs(objectType, interfaceType);
        }

        private static bool TryGetInterfaceArgs(Type type, out Type objectType,
            out Type interfaceType)
        {
            objectType = null;
            interfaceType = null;

            if (type == null) return false;
            if (!type.IsGenericType) return false;

            Type genericType = type.GetGenericTypeDefinition();
            if (genericType == typeof(InterfaceReference<>)) type = type.BaseType;

            if (type?.GetGenericTypeDefinition() != typeof(InterfaceReference<,>)) return false;
            Type[] types = type.GetGenericArguments();
            interfaceType = types[0];
            objectType = types[1];
            return true;
        }

        private static void GetTypeFromList(Type type, out Type objectType, out Type interfaceType)
        {
            objectType = null;
            interfaceType = null;

            Type interfaceTypes = type.GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x
                    .GetGenericTypeDefinition() == typeof(IList<>));

            if (interfaceTypes == null) return;
            Type elementType = interfaceTypes.GetGenericArguments()[0];
            TryGetInterfaceArgs(elementType, out objectType, out interfaceType);
        }
    }

    public struct InterfaceArgs
    {
        public readonly Type ObjectType;
        public readonly Type InterfaceType;

        public InterfaceArgs(Type objectType, Type interfaceType)
        {
            Debug.Assert(typeof(Object).IsAssignableFrom(objectType),
                $"{nameof(objectType)} needs to be of Type {nameof(Object)}!");
            Debug.Assert(interfaceType.IsInterface,
                $"{nameof(interfaceType)} needs to be an interface!");
            this.ObjectType = objectType;
            this.InterfaceType = interfaceType;
        }
    }
}