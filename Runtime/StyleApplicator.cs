using CodeWriter.ViewBinding;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    [RequireComponent(typeof(Style))]
    [AddComponentMenu("View Binding/Style Applicator")]
    public class StyleApplicator : Applicator<Style, ViewVariableString>
    {
        protected override void Apply(Style target, ViewVariableString source)
        {
            target.Apply(source.Value);
        }
    }
}