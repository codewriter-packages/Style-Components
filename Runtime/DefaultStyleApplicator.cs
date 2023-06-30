using CodeWriter.ViewBinding;
using TriInspector;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    [RequireComponent(typeof(Style))]
    [AddComponentMenu("View Binding/[Binding] Style Applicator (Default)")]
    public class DefaultStyleApplicator : ApplicatorBase
    {
        [Required]
        [SerializeField]
        private Style target;

        [SerializeField]
        private string styleName = "Default";

        protected override void Apply()
        {
            if (target == null)
            {
                Debug.LogError($"Null applicator target at '{name}'", this);
                return;
            }

            target.Apply(styleName, link: true);

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(target);
            }
#endif
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            target = GetComponent<Style>();
        }
#endif
    }
}