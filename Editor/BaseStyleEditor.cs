namespace CodeWriter.StyleComponents
{
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    [CustomEditor(typeof(Style), true)]
    public class BaseStyleEditor : Editor
    {
        private const string TargetPropName = "target";
        private const string StyleNamesPropName = "styleNames";
        private const string StyleValuesPropName = "styleValues";

        private static readonly GUIContent NameContent = new GUIContent("Name");
        private static readonly GUIContent ValueContent = new GUIContent("Value");

        private static readonly string[] ExcludedProperties =
            {TargetPropName, StyleNamesPropName, StyleValuesPropName};

        private SerializedProperty _targetProp;
        private SerializedProperty _styleNamesProp;
        private SerializedProperty _styleValuesProp;
        private ReorderableList _styleList;

        private void OnEnable()
        {
            _targetProp = serializedObject.FindProperty(TargetPropName);
            _styleNamesProp = serializedObject.FindProperty(StyleNamesPropName);
            _styleValuesProp = serializedObject.FindProperty(StyleValuesPropName);

            _styleList = CreateStyleList(serializedObject, _styleNamesProp, _styleValuesProp);
        }

        private static ReorderableList CreateStyleList(SerializedObject serializedObject, SerializedProperty namesProp,
            SerializedProperty valuesProp)
        {
            return new ReorderableList(namesProp.serializedObject, namesProp, false, true, true, true)
            {
                drawHeaderCallback = rect => GUI.Label(rect, "Styles"),
                onAddCallback = list =>
                {
                    var index = list.serializedProperty.arraySize;
                    namesProp.InsertArrayElementAtIndex(index);
                    valuesProp.InsertArrayElementAtIndex(index);
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
                    Apply(serializedObject, list.index);
                },
                elementHeight = EditorGUIUtility.singleLineHeight * 2.5f,
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var nameProp = namesProp.GetArrayElementAtIndex(index);
                    var valueProp = valuesProp.GetArrayElementAtIndex(index);

                    EditorGUIUtility.labelWidth -= 50;

                    rect.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.PropertyField(rect, nameProp, NameContent);

                    EditorGUIUtility.labelWidth += 50;

                    EditorGUI.BeginChangeCheck();

                    rect.y += rect.height;
                    EditorGUI.PropertyField(rect, valueProp, ValueContent);

                    if (EditorGUI.EndChangeCheck() && active)
                    {
                        Apply(serializedObject, index);
                    }
                },
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, ExcludedProperties);

            EditorGUILayout.ObjectField(_targetProp);

            if (_targetProp.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Target is null", MessageType.Error);
            }

            GUILayout.Space(10);

            _styleList.DoLayoutList();

            DrawMissingFromSources();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawMissingFromSources()
        {
            var options = new[] {GUILayout.Width(80), GUILayout.Height(36)};

            var baseStyle = (Style) target;
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

        private static void Apply(SerializedObject obj, int index)
        {
            obj.ApplyModifiedProperties();
            var target = (Style) obj.targetObject;
            target.Apply(index);
        }
    }
}