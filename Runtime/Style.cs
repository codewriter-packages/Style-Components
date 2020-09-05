namespace CodeWriter.StyleComponents
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    public abstract class Style : MonoBehaviour
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
}