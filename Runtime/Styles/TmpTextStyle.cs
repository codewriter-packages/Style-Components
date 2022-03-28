using System.Text;

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

        private StringBuilder _stringBuilder;

        protected override void Apply(TMP_Text target, string value)
        {
            if (context != null || extraContexts.Length != 0)
            {
                if (_stringBuilder == null)
                {
                    _stringBuilder = new StringBuilder();
                }

                _stringBuilder.Clear();
                TextFormatUtility.FormatText(_stringBuilder, value, context, extraContexts);
                target.SetText(_stringBuilder);
                _stringBuilder.Clear();
            }
            else
            {
                target.text = value;
            }
        }
    }
}
#endif