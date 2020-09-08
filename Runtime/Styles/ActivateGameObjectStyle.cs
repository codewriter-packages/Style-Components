using CodeWriter.StyleComponents.StyleAssets;

namespace CodeWriter.StyleComponents
{
    using UnityEngine;

    [AddComponentMenu(("Style Components/Activate GameObject Style"))]
    public sealed class ActivateGameObjectStyle : Style<Transform, bool, BoolStyleAsset>
    {
        protected override void Apply(Transform target, bool value) => target.gameObject.SetActive(value);
    }
}