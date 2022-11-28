using CodeWriter.StyleComponents.StyleAssets;
using CodeWriter.ViewBinding;
using UnityEngine;

namespace CodeWriter.StyleComponents.Adapters
{
    [AddComponentMenu("View Binding/Adapters/[Binding] Text Style Adapter")]
    public class TextStyleAdapter : StyleAdapterBase<string, ViewVariableString, TextStyleAsset>
    {
        protected override string DefaultValue { get; } = "";
    }
}