using CodeWriter.ViewBinding;
using CodeWriter.ViewBinding.Applicators;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    [RequireComponent(typeof(Style))]
    [AddComponentMenu("View Binding/Style Applicator (Dynamic)")]
    public class StyleApplicator : ComponentApplicatorBase<Style, ViewVariableString>
    {
        protected override void Apply(Style target, ViewVariableString source)
        {
            target.Apply(source.Value, link: true);
        }
    }
}