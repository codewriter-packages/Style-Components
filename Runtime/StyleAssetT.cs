using System;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    public abstract class StyleAsset<TValue> : StyleAsset
    {
        [SerializeField] private string[] styleNames = default;
        [SerializeField] private TValue[] styleValues = default;

        public sealed override Type ElementType => typeof(TValue);

        public override string[] StyleNames => styleNames;
        public TValue[] StyleValues => styleValues;
    }
}