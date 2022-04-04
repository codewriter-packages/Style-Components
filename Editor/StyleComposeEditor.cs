namespace CodeWriter.StyleComponents
{
    using System;
    using System.Linq;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;
    using Object = UnityEngine.Object;

    [CustomEditor(typeof(StyleCompose))]
    public class StyleComposeEditor : Editor
    {
        private const string ChildrenPropName = "children";
        private const string DataPropName = "data";

        private static readonly string[] ExcludedProperties = {"m_Script", DataPropName, ChildrenPropName};

        private SerializedProperty _dataProp;
        private SerializedProperty _childrenProp;
        private ReorderableList _list;

        private void OnEnable()
        {
            _dataProp = serializedObject.FindProperty(DataPropName);
            _childrenProp = serializedObject.FindProperty(ChildrenPropName);
            _list = CreateList(serializedObject, _dataProp);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, ExcludedProperties);

            DoChildrenGUI();

            _list.DoLayoutList();

            FillData(serializedObject);

            serializedObject.ApplyModifiedProperties();
        }

        private void DoChildrenGUI()
        {
            var headerRect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.label);
            var headerLabelRect = new Rect(headerRect)
            {
                width = EditorGUIUtility.labelWidth
            };
            var headerContentRect = new Rect(headerRect)
            {
                xMin = headerLabelRect.xMax
            };

            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(headerLabelRect, _childrenProp, false);

            if (_childrenProp.isExpanded)
            {
                EditorGUI.indentLevel++;

                for (int i = 0, len = _childrenProp.arraySize; i < len; i++)
                {
                    EditorGUILayout.PropertyField(_childrenProp.GetArrayElementAtIndex(i));
                }

                if (_childrenProp.arraySize == 0)
                {
                    GUILayout.Label("No listeners");
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndDisabledGroup();

            if (GUI.Button(headerContentRect, "Fill Children"))
            {
                FillChildren(serializedObject);
            }

            EditorGUILayout.Space();
        }

        private static ReorderableList CreateList(SerializedObject serializedObject, SerializedProperty property)
        {
            return new ReorderableList(serializedObject, property, false, true, true, true)
            {
                drawHeaderCallback = rect => GUI.Label(rect, "Styles"),
                onSelectCallback = list =>
                {
                    //
                    Apply(serializedObject, list.index);
                },
                elementHeightCallback = index =>
                {
                    var childrenProp = serializedObject.FindProperty(ChildrenPropName);
                    return (childrenProp.arraySize + 1.5f) * EditorGUIUtility.singleLineHeight;
                },
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var dataProp = property.GetArrayElementAtIndex(index);

                    var dataNameProp = dataProp.FindPropertyRelative("name");
                    var dataStylesProp = dataProp.FindPropertyRelative("styles");

                    rect.height = EditorGUIUtility.singleLineHeight;

                    EditorGUIUtility.labelWidth -= 50;

                    EditorGUI.PropertyField(rect, dataNameProp);
                    rect.y += rect.height;

                    EditorGUIUtility.labelWidth += 50;

                    for (int i = 0; i < dataStylesProp.arraySize; i++)
                    {
                        var styleProp = dataStylesProp.GetArrayElementAtIndex(i);

                        var targetProp = styleProp.FindPropertyRelative("target");
                        var styleNameProp = styleProp.FindPropertyRelative("style");

                        if (targetProp.objectReferenceValue == null)
                        {
                            continue;
                        }

                        var child = (Style) targetProp.objectReferenceValue;

                        var valueRect = EditorGUI.PrefixLabel(rect, new GUIContent(child.name));

                        var styleIndex = Array.IndexOf(child.StyleNames, styleNameProp.stringValue);
                        if (styleIndex == -1)
                        {
                            styleIndex = 0;
                        }

                        if (child.StyleNames.Length > 0)
                        {
                            EditorGUI.BeginChangeCheck();

                            styleIndex = EditorGUI.Popup(valueRect, styleIndex, child.StyleNames);
                            if (styleIndex != -1)
                            {
                                styleNameProp.stringValue = child.StyleNames[styleIndex];
                            }

                            if (EditorGUI.EndChangeCheck() && active)
                            {
                                Apply(serializedObject, index);
                            }
                        }
                        else
                        {
                            var color = GUI.color;
                            GUI.color *= Color.yellow;
                            if (GUI.Button(valueRect, "No styles found. Click to select invalid object"))
                            {
                                EditorGUIUtility.PingObject(child);
                            }

                            GUI.color = color;
                        }


                        rect.y += rect.height;
                    }
                }
            };
        }

        private static void Apply(SerializedObject obj, int index)
        {
            UseTarget<StyleCompose>(obj, t =>
            {
                t.Apply(index);
                return true;
            });
        }

        private static void FillData(SerializedObject obj)
        {
            UseTarget<StyleCompose>(obj, target =>
            {
                bool anyDirty = false;
                foreach (var dataEntry in target.Data)
                {
                    //
                    var styles = dataEntry.styles;

                    bool dirty = false;
                    for (var styleIndex = styles.Length - 1; styleIndex >= 0; styleIndex--)
                    {
                        var style = styles[styleIndex];

                        if (style.target == null || Array.IndexOf(target.Children, style.target) == -1)
                        {
                            ArrayUtility.RemoveAt(ref styles, styleIndex);
                            dirty = true;
                        }
                    }

                    foreach (var child in target.Children)
                    {
                        if (child == null)
                        {
                            continue;
                        }

                        if (Array.Find(styles, s => s.target == child) == null)
                        {
                            var styleValue = child.StyleNames.Length > 0 ? child.StyleNames[0] : string.Empty;

                            if (Array.IndexOf(child.StyleNames, dataEntry.name) != -1)
                            {
                                styleValue = dataEntry.name;
                            }

                            ArrayUtility.Add(ref styles, new StyleCompose.TargetStyleData
                            {
                                target = child,
                                style = styleValue,
                            });
                            dirty = true;
                        }
                    }

                    if (!dirty)
                    {
                        continue;
                    }

                    anyDirty = true;

                    Array.Sort(styles, (a, b) =>
                    {
                        var indA = Array.IndexOf(target.Children, a.target);
                        var indB = Array.IndexOf(target.Children, b.target);
                        return indA.CompareTo(indB);
                    });

                    dataEntry.styles = styles;
                }

                if (!anyDirty)
                {
                    return false;
                }

                obj.ApplyModifiedPropertiesWithoutUndo();
                return true;
            });
        }

        private static void FillChildren(SerializedObject obj)
        {
            UseTarget<StyleCompose>(obj, target =>
            {
                target.Children = Enumerable.Empty<Style>()
                    .Union(target.GetComponentsInChildren<Style>(true))
                    .Union(target.GetComponents<Style>())
                    .Where(style => style != null)
                    .Where(style => style != target)
                    .Where(style => style.transform.GetComponentsInParent<StyleCompose>(true)[0] == target)
                    .Where(style => style.GetComponent<StyleApplicator>() == null)
                    .ToArray();

                return true;
            });
        }

        private static void UseTarget<T>(SerializedObject obj, Func<T, bool> action)
            where T : Object
        {
            obj.ApplyModifiedProperties();
            var target = (T) obj.targetObject;
            if (action(target))
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}