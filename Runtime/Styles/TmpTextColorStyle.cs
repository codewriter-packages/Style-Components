#if TEXT_MESH_PRO
namespace CodeWriter.StyleComponents
{
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("Style Components/Tmp Text Color Style")]
    public sealed class TmpTextColorStyle : Style<TMP_Text, Color>
    {
        protected override void Apply(TMP_Text target, Color value) => target.color = value;
    }
}
#endif