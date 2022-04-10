namespace CodeWriter.StyleComponents
{
    using UnityEngine;
    using UnityEngine.UI;
    using StyleAssets;
    using ViewBinding;

    [RequireComponent(typeof(Text))]
    [AddComponentMenu("Style Components/Text Localize Style")]
    public sealed class TextLocalizeStyle : Style<Text, string, TextStyleAsset>
    {
        [SerializeField]
        private ViewContextBase context = default;

        [SerializeField]
        private ViewContextBase[] extraContexts = new ViewContextBase[0];

        protected override void Apply(Text target, string value)
        {
            var formatTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            var localizedTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            try
            {
                formatTextBuilder.AppendFormat(value, context, extraContexts);
                var localizedString = BindingsLocalization.Localize(ref formatTextBuilder);

                localizedTextBuilder.AppendFormat(localizedString, context, extraContexts);
                target.text = localizedTextBuilder.ToString();
            }
            finally
            {
                formatTextBuilder.Dispose();
                localizedTextBuilder.Dispose();
            }
        }
    }
}