using UniMob;

namespace CodeWriter.StyleComponents
{
    using System;
    using UnityEngine;

    public abstract class Style : MonoBehaviour
    {
        public abstract Type ElementType { get; }

        public abstract string[] StyleNames { get; }

        public abstract void Apply(int styleIndex);

        public void Apply(string styleName, bool link = false)
        {
            if (!enabled)
            {
                return;
            }

            var noWatch = link ? null : Atom.NoWatch;
            try
            {
                var index = Array.IndexOf(StyleNames, styleName);
                if (index == -1)
                {
                    Debug.LogError($"No style with name '{styleName}' at '{name}'");
                    index = 0;
                }

                Apply(index);
            }
            finally
            {
                noWatch?.Dispose();
            }
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