using System;
using CodeWriter.ViewBinding;
using TriInspector;
using UnityEngine;

namespace CodeWriter.StyleComponents.Adapters
{
    public abstract class StyleAdapterBase<TResult, TResultVariable, TStyleAsset>
        : SingleResultAdapterBase<TResult, TResultVariable>
        where TResultVariable : ViewVariable<TResult, TResultVariable>, new()
        where TStyleAsset : StyleAsset<TResult>
    {
        [Space]
        [SerializeField]
        private ViewVariableString styleName;

        [Required]
        [SerializeField]
        private TStyleAsset styleAsset;

        protected sealed override TResult Adapt()
        {
            if (styleAsset == null)
            {
                Debug.LogError($"No style asset at '{name}'", this);
                return DefaultValue;
            }

            var style = styleName.Value;
            var styleIndex = Array.IndexOf(styleAsset.StyleNames, style);
            if (styleIndex == -1)
            {
                Debug.LogError($"No style with name '{style}' in '{styleAsset.name}' style asset at '{name}'", this);
                return DefaultValue;
            }

            return styleAsset.StyleValues[styleIndex];
        }

        protected abstract TResult DefaultValue { get; }
    }
}