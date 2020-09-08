using CodeWriter.StyleComponents.StyleAssets;

namespace CodeWriter.StyleComponents
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    [AddComponentMenu("Style Components/Image Color Style")]
    public sealed class ImageColorStyle : Style<Image, Color, ColorStyleAsset>
    {
        protected override void Apply(Image target, Color value) => target.color = value;
    }
}