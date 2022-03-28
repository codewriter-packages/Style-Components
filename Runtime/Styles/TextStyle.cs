using System.Text;

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

        private StringBuilder _stringBuilder;

        protected override void Apply(Text target, string value)
        {
            if (context != null || extraContexts.Length != 0)
            {
                if (_stringBuilder == null)
                {
                    _stringBuilder = new StringBuilder();
                }

                _stringBuilder.Clear();
                TextFormatUtility.FormatText(_stringBuilder, value, context, extraContexts);
                target.text = _stringBuilder.ToString();
                _stringBuilder.Clear();
            }
            else
            {
                target.text = value;
            }
        }
    }
}