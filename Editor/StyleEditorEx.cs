using System.Linq;
using CodeWriter.ViewBinding;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    internal static class StyleEditorEx
    {
        private static readonly GUIContent ContextContent = new GUIContent("Context");

        public static void DrawContextField(SerializedProperty primaryContextProp,
            [CanBeNull] SerializedProperty extraContextsProp)
        {
            var primaryRect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.label);
            var primaryContextRect = new Rect(primaryRect) {width = Mathf.Max(0, primaryRect.width - 100)};
            var primaryActionRect = new Rect(primaryRect) {xMin = primaryContextRect.xMax};

            DoContextField(primaryContextRect, ContextContent, primaryContextProp);

            if (GUI.Button(primaryActionRect, "Fill Context"))
            {
                FillContext(primaryContextProp, extraContextsProp);
            }

            if (extraContextsProp != null)
            {
                for (var i = 0; i < extraContextsProp.arraySize; i++)
                {
                    var extraContextProp = extraContextsProp.GetArrayElementAtIndex(i);

                    var extraRect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.label);
                    var extraContextRect = new Rect(extraRect) {width = Mathf.Max(0, extraRect.width - 100)};

                    DoContextField(extraContextRect, new GUIContent(""), extraContextProp);
                }
            }
        }

        private static void FillContext(SerializedProperty primaryContextProp,
            [CanBeNull] SerializedProperty extraContextsProp)
        {
            if (primaryContextProp.serializedObject.targetObject is MonoBehaviour mb)
            {
                var primaryContext = mb.GetComponentInParent<ViewContext>();
                primaryContextProp.objectReferenceValue = primaryContext;

                if (extraContextsProp != null)
                {
                    var extraContexts = Enumerable.Empty<ViewContextBase>()
                        .Concat(mb.GetComponentsInParent<ViewContextBase>())
                        .Where(it => it != null && it != mb && it != primaryContext)
                        .ToList();

                    extraContextsProp.arraySize = extraContexts.Count;
                    for (var i = 0; i < extraContexts.Count; i++)
                    {
                        extraContextsProp.GetArrayElementAtIndex(i).objectReferenceValue = extraContexts[i];
                    }
                }

                primaryContextProp.serializedObject.ApplyModifiedProperties();
            }
        }

        private static void DoContextField(Rect position, GUIContent label, SerializedProperty property)
        {
            var labelRect = new Rect(position) {width = EditorGUIUtility.labelWidth};
            var contentRect = new Rect(position) {xMin = labelRect.xMax};

            var oldEnabled = GUI.enabled;
            GUI.enabled = false;

            GUI.Label(labelRect, label);
            EditorGUI.PropertyField(contentRect, property, GUIContent.none);

            GUI.enabled = oldEnabled;
        }
    }
}