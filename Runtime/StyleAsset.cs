using System;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    public abstract class StyleAsset : ScriptableObject, IStyle
    {
        [SerializeField] private StyleNamesSource[] namesSources = new StyleNamesSource[0];
        
        public abstract string[] StyleNames { get; }

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