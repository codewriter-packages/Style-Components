namespace CodeWriter.StyleComponents
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    public abstract class Style : MonoBehaviour
    {
        [SerializeField] private StyleContext context = default;

        public abstract string[] StyleNames { get; }

        [CanBeNull] public StyleContext Context => context;

        public abstract void Apply(int styleIndex);

        protected virtual void OnEnable() {
            
        }

        public void Apply(string styleName)
        {
            if (!enabled) {
                return;
            }
        
            var index = Array.IndexOf(StyleNames, styleName);
            if (index == -1)
            {
                Debug.LogError($"No style with name '{styleName}' at '{name}'");
                index = 0;
            }

            Apply(index);
        }

        protected bool TryGetContextVariables(out string[] variables)
        {
            if (Context != null && Context.VariablesArray != null)
            {
                variables = Context.VariablesArray;
                return true;
            }

            variables = default;
            return false;
        }

        protected virtual void Reset()
        {
        }
    }
}