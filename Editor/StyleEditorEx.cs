namespace CodeWriter.StyleComponents {
    using System;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    internal static class StyleEditorEx {
        private static readonly GUIContent ContextContent = new GUIContent("Context");

        public static void DrawContextField(SerializedProperty contextProp) {
            var context = contextProp.objectReferenceValue;
            if (context == null) {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(ContextContent);
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.Label("None");
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Use Context", EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                    UseTarget<MonoBehaviour>(contextProp.serializedObject, it => {
                        contextProp.objectReferenceValue = it.GetComponentInParent<StyleContext>();
                        return true;
                    });
                }

                GUILayout.EndHorizontal();
            }
            else {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(ContextContent);
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.TextField(context.name);
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Reset Context", EditorStyles.miniButton, GUILayout.ExpandWidth(false))) {
                    UseTarget<MonoBehaviour>(contextProp.serializedObject, it => {
                        contextProp.objectReferenceValue = null;
                        return true;
                    });
                }

                GUILayout.EndHorizontal();
            }
        }

        private static void UseTarget<T>(SerializedObject obj, Func<T, bool> action)
            where T : Object {
            obj.ApplyModifiedProperties();
            var target = (T) obj.targetObject;
            if (action(target)) {
                EditorUtility.SetDirty(target);
            }
        }
    }
}