namespace CodeWriter.StyleComponents
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Style), true)]
    public class StyleEditor : Editor
    {
        private const string TargetPropName = "target";
        private const string AssetPropName = "asset";
        private const string ContextPropName = "context";

        private static readonly string[] ExcludedProperties =
        {
            "m_Script",
            TargetPropName,
            AssetPropName,
            ContextPropName,
            StyleListEditor.StyleNamesPropName,
            StyleListEditor.StyleValuesPropName
        };

        private SerializedProperty _targetProp;
        private SerializedProperty _assetProp;
        private SerializedProperty _contextProp;
        private StyleListEditor _selfStyleListEditor;

        private SerializedObject _styleAssetSerializedObject;
        private StyleListEditor _assetStyleListEditor;

        private void OnEnable()
        {
            _targetProp = serializedObject.FindProperty(TargetPropName);
            _assetProp = serializedObject.FindProperty(AssetPropName);
            _contextProp = serializedObject.FindProperty(ContextPropName);
            _selfStyleListEditor = new StyleListEditor(serializedObject, Apply, true);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            StyleEditorEx.DrawContextField(_contextProp);

            DrawPropertiesExcluding(serializedObject, ExcludedProperties);

            EditorGUILayout.ObjectField(_targetProp);

            if (_targetProp.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Target is null", MessageType.Error);
            }

            GUILayout.Space(10);

            if (_assetProp != null)
            {
                EditorGUILayout.ObjectField(_assetProp);
            }

            if (_assetProp?.objectReferenceValue != null)
            {
                if (_styleAssetSerializedObject?.targetObject != _assetProp.objectReferenceValue)
                {
                    _styleAssetSerializedObject?.Dispose();

                    _styleAssetSerializedObject = new SerializedObject(_assetProp.objectReferenceValue);
                    _assetStyleListEditor = new StyleListEditor(_styleAssetSerializedObject, Apply, false);
                }

                _assetStyleListEditor.DoLayout();
            }
            else
            {
                _styleAssetSerializedObject?.Dispose();
                _styleAssetSerializedObject = null;

                _selfStyleListEditor.DoLayout();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void Apply(int index)
        {
            serializedObject.ApplyModifiedProperties();
            _styleAssetSerializedObject?.ApplyModifiedProperties();

            ((Style) target).Apply(index);
        }
    }
}