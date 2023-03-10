using UnityEditor;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    [CustomEditor(typeof(StyleAsset), true)]
    public class StyleAssetEditor : Editor
    {
        private static readonly string[] ExcludedProperties =
        {
            "m_Script",
            StyleListEditor.StyleNamesPropName,
            StyleListEditor.StyleValuesPropName
        };

        private StyleListEditor _styleListEditor;

        private void OnEnable()
        {
            var elementType = ((StyleAsset) target).ElementType;

            _styleListEditor = new StyleListEditor(serializedObject, Apply, true, elementType);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, ExcludedProperties);

            GUILayout.Space(10);

            _styleListEditor.DoLayout();
            serializedObject.ApplyModifiedProperties();
        }

        private void Apply(int styleIndex)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}