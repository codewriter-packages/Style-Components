using CodeWriter.ViewBinding;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    [RequireComponent(typeof(Style))]
    [AddComponentMenu("View Binding/Default Style Applicator")]
    public class DefaultStyleApplicator : ApplicatorBase
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Required]
#endif
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

            target.Apply(styleName);

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