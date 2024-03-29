using System;

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
        private const string ExtraContextsPropName = "extraContexts";

        private static readonly string[] ExcludedProperties =
        {
            "m_Script",
            TargetPropName,
            AssetPropName,
            ContextPropName,
            ExtraContextsPropName,
            StyleListEditor.StyleNamesPropName,
            StyleListEditor.StyleValuesPropName
        };

        private Type _elementType;
        
        private SerializedProperty _assetProp;
        private SerializedProperty _contextProp;
        private SerializedProperty _extraContextsProp;
        private StyleListEditor _selfStyleListEditor;

        private SerializedObject _styleAssetSerializedObject;
        private StyleListEditor _assetStyleListEditor;

        private void OnEnable()
        {
            _elementType = ((Style) target).ElementType;
            
            _assetProp = serializedObject.FindProperty(AssetPropName);
            _contextProp = serializedObject.FindProperty(ContextPropName);
            _extraContextsProp = serializedObject.FindProperty(ExtraContextsPropName);
            _selfStyleListEditor = new StyleListEditor(serializedObject, Apply, true, _elementType);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_contextProp != null)
            {
                StyleEditorEx.DrawContextField(_contextProp, _extraContextsProp);
            }

            DrawPropertiesExcluding(serializedObject, ExcludedProperties);

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
                    _assetStyleListEditor = new StyleListEditor(_styleAssetSerializedObject, Apply, false, _elementType);
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