namespace CodeWriter.StyleComponents
{
    using System;
    using UnityEngine;

    public class StyleCompose : Style
    {
        [SerializeField] private Style[] children = default;
        [SerializeField] private StyleData[] data = default;

        [Serializable]
        public class StyleData
        {
            public string name;
            public TargetStyleData[] styles = default;
        }

        [Serializable]
        public class TargetStyleData
        {
            public Style target = default;
            public string style = default;
        }

        public Style[] Children
        {
            get => children;
            set => children = value;
        }

        public StyleData[] Data
        {
            get => data;
            set => data = value;
        }

        public override string[] StyleNames => Array.ConvertAll(data, d => d.name);

        public sealed override void Apply(int styleIndex)
        {
            if (styleIndex < 0 || styleIndex >= data.Length)
            {
                Debug.LogError($"No style with index '{styleIndex}' at '{name}'");
                styleIndex = 0;
            }

            foreach (var child in data[styleIndex].styles)
            {
                if (child.target == null)
                {
                    Debug.LogError($"Null child at '{name}'");
                    continue;
                }

                child.target.Apply(child.style);
            }
        }
    }
}