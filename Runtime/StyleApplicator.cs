using CodeWriter.ViewBinding;
using CodeWriter.ViewBinding.Applicators;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    [RequireComponent(typeof(Style))]
    [AddComponentMenu("View Binding/Style Applicator")]
    public class StyleApplicator : ComponentApplicatorBase<Style, ViewVariableString>
    {
        protected override void Apply(Style target, ViewVariableString source)
        {
            target.Apply(source.Value);
        }

#if UNITY_EDITOR
        public override void OnEditorContextVariableChanged(ViewVariable variable)
        {
            Apply();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            var target = GetTarget();
            if (target != null)
            {
                target.EditorTrackModifications(this);
            }
        }
#endif
    }
}