using CodeWriter.StyleComponents.StyleAssets;

namespace CodeWriter.StyleComponents
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    [AddComponentMenu("Style Components/Image Sprite Style")]
    public sealed class ImageSpriteStyle : Style<Image, Sprite, SpriteStyleAsset>
    {
        protected override void Apply(Image target, Sprite value) => target.sprite = value;
    }
}