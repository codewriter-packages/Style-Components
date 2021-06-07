#if TEXT_MESH_PRO

namespace CodeWriter.StyleComponents
{
    using TMPro;
    using UnityEngine;
    using StyleAssets;
    using ViewBinding;

    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("Style Components/Tmp Text Style")]
    public sealed class TmpTextStyle : Style<TMP_Text, string, TextStyleAsset>
    {
        [SerializeField]
        private ViewContextBase context = default;

        [SerializeField]
        private ViewContextBase[] extraContexts = new ViewContextBase[0];

        protected override void Apply(TMP_Text target, string value)
        {
            if (context != null || extraContexts.Length != 0)
            {
                target.SetText(TextFormatUtility.FormatText(value, context, extraContexts));
            }
            else
            {
                target.text = value;
            }
        }
    }
}
#endif