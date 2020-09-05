#if I2_LOCALIZATION
namespace CodeWriter.StyleComponents
{
    using UniGreenModules.UniCore.Runtime.ComponentStyle;
    using UnityEngine;

    [RequireComponent(typeof(I2.Loc.Localize))]
    [AddComponentMenu("Style Components/I2 Localize Text Style")]
    public sealed class I2LocalizationTextStyle : Style<I2.Loc.Localize, string>
    {
        protected override void Apply(I2.Loc.Localize target, string value)
        {
            target.Term = value;
            target.OnLocalize(true);
        }
    }
}
#endif