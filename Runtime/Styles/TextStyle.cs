namespace CodeWriter.StyleComponents
{
    using UnityEngine;
    using UnityEngine.UI;
    using StyleAssets;
    using ViewBinding;

    [RequireComponent(typeof(Text))]
    [AddComponentMenu("Style Components/Text Style")]
    public sealed class TextStyle : Style<Text, string, TextStyleAsset>
    {
        [SerializeField]
        private ViewContextBase context = default;

        [SerializeField]
        private ViewContextBase[] extraContexts = new ViewContextBase[0];

        protected override void Apply(Text target, string value)
        {
            if (context != null || extraContexts.Length != 0)
            {
                target.text = TextFormatUtility.FormatText(value, context, extraContexts).ToString();
            }
            else
            {
                target.text = value;
            }
        }
    }
}