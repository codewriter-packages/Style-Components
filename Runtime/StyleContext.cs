using System.Collections.Generic;
using UniMob;

namespace CodeWriter.StyleComponents
{
    using System;
    using UnityEngine;
    using ViewBinding;

    [AddComponentMenu("")]
    public class StyleContext : ViewContext
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.TableList(AlwaysExpanded = true, ShowPaging = false)]
#endif
        [HideInInspector]
        [SerializeField]
        private Variable[] variables = new Variable[0];

        [Serializable]
        private class Variable
        {
            public string key;
            public string defaultValue;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            MigrateVariables();

            if (variables.Length != 0)
            {
                variables = new Variable[0];
            }
        }
#endif

        private void Awake()
        {
            MigrateVariables();
        }

        public void SetVariable(string key, string value)
        {
            var viewVariable = FindVariable<ViewVariableString>(key);
            if (viewVariable != null)
            {
                viewVariable.SetValue(value);
            }
            else
            {
                var obj = gameObject;
                Debug.LogError($"Key {key} not exists at {obj.name}", obj);
            }
        }

        private void MigrateVariables()
        {
            foreach (var variable in variables)
            {
                if (FindVariable(variable.key) != null)
                {
                    continue;
                }

                var viewVariable = new ViewVariableString();
                viewVariable.SetContext(this);
                viewVariable.SetName(variable.key);

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    viewVariable.SetValueEditorOnly(variable.defaultValue);
                }
                else
#endif
                {
                    viewVariable.SetValue(variable.defaultValue);
                }

                UnsafeRegisterVariable(viewVariable);
            }
        }
    }
}