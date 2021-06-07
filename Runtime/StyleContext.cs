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

        private Dictionary<string, MutableAtom<string>> _atoms = new Dictionary<string, MutableAtom<string>>();

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
            if (_atoms.TryGetValue(key, out var atom))
            {
                atom.Value = value;
            }
            else
            {
                var viewVariable = FindVariable<ViewVariableString>(key);
                if (viewVariable != null)
                {
                    atom = Atom.Value(value);
                    _atoms.Add(key, atom);
                    viewVariable.SetSource(atom);
                }
                else
                {
                    var obj = gameObject;
                    Debug.LogError($"Key {key} not exists at {obj.name}", obj);
                }
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
                    var atom = Atom.Value(variable.defaultValue);
                    _atoms.Add(variable.key, atom);
                    viewVariable.SetSource(atom);
                }

                UnsafeRegisterVariable(viewVariable);
            }
        }
    }
}