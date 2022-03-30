using System;
using CodeWriter.ViewBinding;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeWriter.StyleComponents
{
    public class StyleContextMenu
    {
        [MenuItem("CONTEXT/StyleContext/Convert To View Context")]
        private static void ConvertToViewContext(MenuCommand command)
        {
            ConvertScriptTo<ViewContext>(command.context as MonoBehaviour);
        }

        private static void ConvertScriptTo<T>(Object target) where T : Object
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var so = new SerializedObject(target);
            so.Update();

            var monoTarget = target as MonoBehaviour;
            var oldEnabled = monoTarget != null && monoTarget.enabled;

            if (monoTarget != null)
            {
                monoTarget.enabled = false;
            }

            foreach (var script in Resources.FindObjectsOfTypeAll<MonoScript>())
            {
                if (script.GetClass() != typeof(T))
                    continue;

                so.FindProperty("m_Script").objectReferenceValue = script;
                so.ApplyModifiedProperties();
                so.Dispose();
                break;
            }

            if (monoTarget != null)
            {
                monoTarget.enabled = oldEnabled;
            }
        }
    }
}