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

        private static readonly GUIContent EmptyContent = new GUIContent();
        private static readonly GUIContent NameContent = new GUIContent("Name");
        private static readonly GUIContent ValueContent = new GUIContent("Value");

        private readonly SerializedObject _serializedObject;
        private readonly Action<int> _apply;
        private readonly bool _editable;
        private readonly Type _elementType;
        private readonly SerializedProperty _styleNamesProp;
        private readonly SerializedProperty _styleValuesProp;
        private readonly ReorderableList _styleList;
        private readonly GUIContent _handleIconContent;

        public StyleListEditor(SerializedObject serializedObject, Action<int> apply, bool editable, Type elementType)
        {
            _serializedObject = serializedObject;
            _apply = apply;
            _editable = editable;
            _elementType = elementType;
            _styleNamesProp = serializedObject.FindProperty(StyleNamesPropName);
            _styleValuesProp = serializedObject.FindProperty(StyleValuesPropName);

            _styleList = CreateStyleList(_styleNamesProp, _styleValuesProp);
            _handleIconContent = EditorGUIUtility.IconContent("Toolbar Minus");
        }

        public void DoLayout()
        {
            _styleList.DoLayoutList();
        }

        private ReorderableList CreateStyleList(SerializedProperty namesProp, SerializedProperty valuesProp)
        {
            var isSprite = typeof(Sprite).IsAssignableFrom(_elementType);

            float GetSpritePreviewSize()
            {
                return namesProp.arraySize <= 15 ? 50 : 30;
            }

            return new ReorderableList(namesProp.serializedObject, namesProp, true, true, true, true)
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
                onReorderCallbackWithDetails = (list, oldIndex, newIndex) =>
                {
                    valuesProp.MoveArrayElement(oldIndex, newIndex);
                },
                onSelectCallback = list =>
                {
                    //
                    _apply(list.index);
                },
                elementHeight = isSprite ? GetSpritePreviewSize() : EditorGUIUtility.singleLineHeight,
                drawElementBackgroundCallback = (rect, index, active, focused) =>
                {
                    if (focused)
                    {
                        ReorderableList.defaultBehaviours.DrawElementBackground(rect, index, active, true, false);
                        return;
                    }

                    var valueProp = valuesProp.GetArrayElementAtIndex(index);

                    if (valueProp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (valueProp.objectReferenceValue == null)
                        {
                            DrawBackground(rect, new Color(0.95f, 0.05f, 0.05f, 0.5f));
                            return;
                        }
                    }

                    if (index % 2 != 0)
                    {
                        DrawBackground(rect, new Color(1, 1, 1, 0.3f));
                    }
                },
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var nameProp = namesProp.GetArrayElementAtIndex(index);
                    var valueProp = valuesProp.GetArrayElementAtIndex(index);

                    var oldEnabled = GUI.enabled;
                    GUI.enabled = _editable;

                    if (isSprite)
                    {
                        var previewSize = GetSpritePreviewSize();
                        var previewRect = new Rect(rect)
                        {
                            xMin = rect.xMax - previewSize - 3,
                        };

                        DrawTexturePreview(previewRect, (Sprite) valueProp.objectReferenceValue);

                        rect.xMax -= previewSize + 6;
                    }

                    rect.height = EditorGUIUtility.singleLineHeight;

                    var nameRect = new Rect(rect)
                    {
                        xMax = rect.xMin + EditorGUIUtility.labelWidth,
                    };
                    var valueRect = new Rect(rect)
                    {
                        xMin = nameRect.xMax + 3,
                    };

                    EditorGUI.PropertyField(nameRect, nameProp, EmptyContent);

                    EditorGUI.BeginChangeCheck();

                    EditorGUI.PropertyField(valueRect, valueProp, EmptyContent);

                    GUI.enabled = oldEnabled;

                    if (EditorGUI.EndChangeCheck() && active)
                    {
                        _apply(index);
                    }
                },
            };
        }

        private static void DrawTexturePreview(Rect position, Sprite sprite, float padding = 3)
        {
            if (sprite == null)
            {
                return;
            }

            position.xMin += padding;
            position.yMin += padding;
            position.xMax -= padding;
            position.yMax -= padding;

            var texRect = sprite.textureRect;
            var fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
            var invFullSize = new Vector2(1f / fullSize.x, 1f / fullSize.y);
            var size = new Vector2(texRect.width, texRect.height);
            var invSize = new Vector2(1f / size.x, 1f / size.y);

            var coords = new Rect(
                Vector2.Scale(texRect.position, invFullSize),
                Vector2.Scale(texRect.size, invFullSize));
            var ratio = Vector2.Scale(position.size, invSize);
            var minRatio = Mathf.Min(ratio.x, ratio.y);

            var center = position.center;
            position.size = size * minRatio;
            position.center = center;

            GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
        }

        private static void DrawBackground(Rect rect, Color color)
        {
            var oldColor = GUI.color;
            GUI.color *= color;
            GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
            GUI.color = oldColor;
        }
    }
}