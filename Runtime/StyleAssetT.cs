using UnityEngine;

namespace CodeWriter.StyleComponents
{
    public abstract class StyleAsset<TValue> : StyleAsset
    {
        [SerializeField] private string[] styleNames = default;
        [SerializeField] private TValue[] styleValues = default;

        public override string[] StyleNames => styleNames;
        public TValue[] StyleValues => styleValues;
    }
}