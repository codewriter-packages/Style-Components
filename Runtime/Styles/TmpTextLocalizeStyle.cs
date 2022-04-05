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

        protected override void Apply(TMP_Text target, string value)
        {
            var formatTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            var localizedTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
            try
            {
                TextFormatUtility.FormatText(ref formatTextBuilder, value, context, extraContexts);
                var localizedString = BindingsLocalization.Localize(ref formatTextBuilder);

                TextFormatUtility.FormatText(ref localizedTextBuilder, localizedString, context, extraContexts);
                target.SetText(localizedTextBuilder.RawCharArray, 0, localizedTextBuilder.Length);
            }
            finally
            {
                formatTextBuilder.Dispose();
                localizedTextBuilder.Dispose();
            }
        }
    }
}

#endif