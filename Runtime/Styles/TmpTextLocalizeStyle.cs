using System.Text;

#if TEXT_MESH_PRO

namespace CodeWriter.StyleComponents
{
    using UnityEngine;
    using StyleAssets;
    using ViewBinding;
    using TMPro;

    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("Style Components/Tmp Text Localize Style")]
    public sealed class TmpTextLocalizeStyle : Style<TMP_Text, string, TextStyleAsset>
    {
        [SerializeField]
        private ViewContextBase context = default;

        [SerializeField]
        private ViewContextBase[] extraContexts = new ViewContextBase[0];

        private StringBuilder _stringBuilder;

        protected override void Apply(TMP_Text target, string value)
        {
            if (_stringBuilder == null)
            {
                _stringBuilder = new StringBuilder();
            }

            TextFormatUtility.FormatText(_stringBuilder, value, context, extraContexts);
            var localizedString = BindingsLocalization.Localize(_stringBuilder);

            TextFormatUtility.FormatText(_stringBuilder, localizedString, context, extraContexts);
            target.SetText(_stringBuilder);
        }
    }
}

#endif