namespace CodeWriter.StyleComponents
{
    using UnityEngine;

    public abstract class Style<TTarget, TValue> : Style
        where TTarget : Component
    {
        [SerializeField] private TTarget target = default;

        [SerializeField] private string[] styleNames = default;
        [SerializeField] private TValue[] styleValues = default;

        public override string[] StyleNames => styleNames;

        protected abstract void Apply(TTarget target, TValue value);

        public sealed override void Apply(int styleIndex)
        {
            if (target == null)
            {
                Debug.LogError($"Style target is null on '{name}'");
            }

            if (styleValues.Length == 0)
            {
                Debug.LogError($"No styles at '{name}'");
                return;
            }

            if (styleIndex < 0 || styleIndex >= styleValues.Length)
            {
                Debug.LogError($"No style with index '{styleIndex}' at '{name}'");
                styleIndex = 0;
            }

            Apply(target, styleValues[styleIndex]);

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        protected bool TryGetContextVariables(out string[] variables)
        {
            if (Context != null && Context.VariablesArray != null)
            {
                variables = Context.VariablesArray;
                return true;
            }

            variables = default;
            return false;
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            target = GetComponent<TTarget>();
        }
#endif
    }
}