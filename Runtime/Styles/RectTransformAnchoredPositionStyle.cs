namespace CodeWriter.StyleComponents
{
    using StyleAssets;
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Style Components/RectTransform Anchored Position Style")]
    public class RectTransformAnchoredPositionStyle : Style<RectTransform, Vector2, Vector2StyleAsset>
    {
        protected override void Apply(RectTransform target, Vector2 value) => target.anchoredPosition = value;
    }
}