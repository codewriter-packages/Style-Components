namespace CodeWriter.StyleComponents
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using Object = UnityEngine.Object;

    internal static class EditorUtilities
    {
        public static void ConvertScriptTo<T>(Object target) where T : Object
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