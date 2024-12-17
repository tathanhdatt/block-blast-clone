using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Dt.Attribute.Editor
{
    [InitializeOnLoad]
    public static class RequiredValidator
    {
        static RequiredValidator()
        {
            EditorApplication.playModeStateChanged += HandleOnPlayModeStateChanged;
        }

        private static void HandleOnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                DebugWarning();
            }
        }

        private static void DebugWarning()
        {
            bool hasNull = false;
            MonoBehaviour[] monoBehaviours =
                Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (MonoBehaviour monoBehaviour in monoBehaviours)
            {
                FieldInfo[] fieldInfos = monoBehaviour
                    .GetType()
                    .GetFields(BindingFlags.Instance |
                               BindingFlags.NonPublic |
                               BindingFlags.Public);
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    RequiredAttribute attribute = fieldInfo.GetCustomAttribute<RequiredAttribute>();
                    if (attribute is null) continue;
                    object value = fieldInfo.GetValue(monoBehaviour);
                    if (!value.Equals(null)) continue;
                    Debug.LogError($"{fieldInfo.Name} is null!", monoBehaviour);
                    hasNull = true;
                }
            }

            if (hasNull)
            {
                EditorApplication.ExitPlaymode();
            }
        }
    }
}