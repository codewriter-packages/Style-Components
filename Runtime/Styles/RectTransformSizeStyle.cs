namespace CodeWriter.StyleComponents
{
    using StyleAssets;
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Style Components/RectTransform Size Style")]
    public class RectTransformSizeStyle : Style<RectTransform, Vector2, Vector2StyleAsset>
    {
        protected override void Apply(RectTransform target, Vector2 value) => target.sizeDelta = value;
    }
}