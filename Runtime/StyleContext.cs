namespace CodeWriter.StyleComponents
{
    using System;
    using UnityEngine;
    using ViewBinding;

    [Obsolete("StyleContext is obsolete. Use ViewContext instead")]
    [AddComponentMenu("")]
    public class StyleContext : ViewContext
    {
        [SerializeField]
        private Variable[] variables = new Variable[0];

        [Serializable]
        private class Variable
        {
            public string key;
            public string defaultValue;
        }

        protected override int VariablesCount => base.VariablesCount + (variables?.Length ?? 0);

        protected override ViewVariable GetVariable(int index)
        {
            MigrateVariables();
            return base.GetVariable(index);
        }

        public void SetVariable(string key, string value)
        {
            MigrateVariables();

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
            if (variables == null || variables.Length == 0)
            {
                return;
            }

            var copy = new Variable[variables.Length];
            Array.Copy(variables, copy, variables.Length);

            variables = null;

            foreach (var variable in copy)
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

        [ContextMenu("Migrate Legacy Variables", true)]
        private bool CanMigrateVariablesAndClear()
        {
            return variables != null && variables.Length > 0;
        }

        [ContextMenu("Migrate Legacy Variables")]
        private void MigrateVariablesAndClear()
        {
            MigrateVariables();
            variables = new Variable[0];
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}