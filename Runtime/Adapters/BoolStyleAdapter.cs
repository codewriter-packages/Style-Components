using CodeWriter.StyleComponents.StyleAssets;
using CodeWriter.ViewBinding;
using UnityEngine;

namespace CodeWriter.StyleComponents.Adapters {
    [AddComponentMenu("View Binding/Adapters/[Binding] Bool Style Adapter")]
    public class BoolStyleAdapter : StyleAdapterBase<bool, ViewVariableBool, BoolStyleAsset>
    {
        protected override bool DefaultValue { get; } = false;
    }
}