using CodeWriter.StyleComponents.StyleAssets;

namespace CodeWriter.StyleComponents
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Text))]
    [AddComponentMenu("Style Components/Text Color Style")]
    public sealed class TextColorStyle : Style<Text, Color, ColorStyleAsset>
    {
        protected override void Apply(Text target, Color value) => target.color = value;
    }
}