namespace CodeWriter.StyleComponents
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;
    using ViewBinding;

    public abstract class Style : MonoBehaviour
    {
        [SerializeField] private ViewContext context = default;

        public abstract string[] StyleNames { get; }

        [CanBeNull] public ViewContext Context => context;

        public abstract void Apply(int styleIndex);

        public void Apply(string styleName)
        {
            if (!enabled)
            {
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

        protected virtual void Reset()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnValidate()
        {
        }
    }
}