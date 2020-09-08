using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    public class StyleListEditor
    {
        public const string StyleNamesPropName = "styleNames";
        public const string StyleValuesPropName = "styleValues";

        private static readonly GUIContent NameContent = new GUIContent("Name");
        private static readonly GUIContent ValueContent = new GUIContent("Value");

        private readonly SerializedObject _serializedObject;
        private readonly Action<int> _apply;
        private readonly bool _editable;
        private readonly SerializedProperty _styleNamesProp;
        private readonly SerializedProperty _styleValuesProp;
        private readonly ReorderableList _styleList;

        public StyleListEditor(SerializedObject serializedObject, Action<int> apply, bool editable)
        {
            if (!(serializedObject.targetObject is IStyle))
            {
                throw new ArgumentException("SerializedObject must be IStyle", nameof(serializedObject));
            }

            _serializedObject = serializedObject;
            _apply = apply;
            _editable = editable;
            _styleNamesProp = serializedObject.FindProperty(StyleNamesPropName);
            _styleValuesProp = serializedObject.FindProperty(StyleValuesPropName);

            _styleList = CreateStyleList(_styleNamesProp, _styleValuesProp);
        }

        public void DoLayout()
        {
            _styleList.DoLayoutList();

            DrawMissingFromSources();
        }

        private void DrawMissingFromSources()
        {
            var options = new[] {GUILayout.Width(80), GUILayout.Height(36)};

            var baseStyle = (IStyle) _serializedObject.targetObject;
            baseStyle.FindMissingFromSources(item =>
            {
                var msg = $"Missing {item}";
                GUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox(msg, MessageType.Error);

                if (GUILayout.Button("Add", options))
                {
                    var index = _styleList.serializedProperty.arraySize;
                    _styleValuesProp.InsertArrayElementAtIndex(index);
                    _styleNamesProp.InsertArrayElementAtIndex(index);
                    _styleNamesProp.GetArrayElementAtIndex(index).stringValue = item;
                }

                GUILayout.EndHorizontal();
            });
        }

        private ReorderableList CreateStyleList(SerializedProperty namesProp, SerializedProperty valuesProp)
        {
            return new ReorderableList(namesProp.serializedObject, namesProp, false, true, true, true)
            {
                drawHeaderCallback = rect => GUI.Label(rect, "Styles"),
                onAddCallback = list =>
                {
                    var index = list.serializedProperty.arraySize;
                    namesProp.InsertArrayElementAtIndex(index);
                    valuesProp.InsertArrayElementAtIndex(index);

                    if (valuesProp.GetArrayElementAtIndex(index).propertyType == SerializedPropertyType.Color)
                    {
                        valuesProp.GetArrayElementAtIndex(index).colorValue = Color.black;
                    }
                },
                onRemoveCallback = list =>
                {
                    var index = list.index;

                    if (valuesProp.GetArrayElementAtIndex(index).propertyType == SerializedPropertyType.ObjectReference)
                    {
                        valuesProp.GetArrayElementAtIndex(index).objectReferenceValue = null;
                    }

                    valuesProp.DeleteArrayElementAtIndex(index);
                    namesProp.DeleteArrayElementAtIndex(index);
                },
                onSelectCallback = list =>
                {
                    //
                    _apply(list.index);
                },
                elementHeight = EditorGUIUtility.singleLineHeight * 2.5f,
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var nameProp = namesProp.GetArrayElementAtIndex(index);
                    var valueProp = valuesProp.GetArrayElementAtIndex(index);

                    var oldEnabled = GUI.enabled;
                    GUI.enabled = _editable;

                    EditorGUIUtility.labelWidth -= 50;

                    rect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(rect, nameProp, NameContent);

                    EditorGUIUtility.labelWidth += 50;

                    EditorGUI.BeginChangeCheck();

                    rect.y += rect.height;
                    EditorGUI.PropertyField(rect, valueProp, ValueContent);

                    GUI.enabled = oldEnabled;

                    if (EditorGUI.EndChangeCheck() && active)
                    {
                        _apply(index);
                    }
                },
            };
        }
    }
}