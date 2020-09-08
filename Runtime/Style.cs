namespace CodeWriter.StyleComponents
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    public abstract class Style : MonoBehaviour, IStyle
    {
        [SerializeField] private StyleContext context = default;
        [SerializeField] private StyleNamesSource[] namesSources = new StyleNamesSource[0];

        public abstract string[] StyleNames { get; }

        [CanBeNull] public StyleContext Context => context;

        public abstract void Apply(int styleIndex);

        public void Apply(string styleName)
        {
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

        public void FindMissingFromSources(Action<string> found)
        {
            foreach (var source in namesSources)
            {
                if (source == null) continue;

                foreach (var item in source.Names)
                {
                    if (Array.IndexOf(StyleNames, item) == -1)
                    {
                        found(item);
                    }
                }
            }
        }
    }

    public interface IStyle
    {
        void FindMissingFromSources(Action<string> found);
    }
}