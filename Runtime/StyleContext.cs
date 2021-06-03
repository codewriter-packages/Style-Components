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

        protected override void OnValidate()
        {
            base.OnValidate();

            MigrateVariables();

            if (variables.Length != 0)
            {
                variables = new Variable[0];
            }
        }

        private void Awake()
        {
            MigrateVariables();
        }

        public void SetVariable(string key, string value)
        {
            var viewVariable = FindVariable(key);
            if (viewVariable is ViewVariableString viewVariableString)
            {
                viewVariableString.Value = value;
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
                viewVariable.Value = variable.defaultValue;
                UnsafeRegisterVariable(viewVariable);
            }
        }
    }
}