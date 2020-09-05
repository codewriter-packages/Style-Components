#if TEXT_MESH_PRO
namespace CodeWriter.StyleComponents
{
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("Style Components/Tmp Text Style")]
    public sealed class TmpTextStyle : Style<TMP_Text, string>
    {
        protected override void Apply(TMP_Text target, string value)
        {
            target.text = TryGetContextVariables(out var variables)
                ? StringUtils.Replace(value, variables)
                : value;
        }
    }
}
#endif