using System.Text;

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

        private StringBuilder _stringBuilder;

        protected override void Apply(Text target, string value)
        {
            if (_stringBuilder == null)
            {
                _stringBuilder = new StringBuilder();
            }

            TextFormatUtility.FormatText(_stringBuilder, value, context, extraContexts);
            var localizedString = BindingsLocalization.Localize(_stringBuilder);

            TextFormatUtility.FormatText(_stringBuilder, localizedString, context, extraContexts);
            target.text = _stringBuilder.ToString();
        }
    }
}