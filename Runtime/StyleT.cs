using System;

namespace CodeWriter.StyleComponents
{
    using UnityEngine;

    public abstract class Style<TTarget, TValue> : Style
        where TTarget : Component
    {
        [SerializeField] private TTarget target = default;

        [SerializeField] private string[] styleNames = default;
        [SerializeField] private TValue[] styleValues = default;

        public sealed override Type ElementType => typeof(TValue);

        public override string[] StyleNames => styleNames;

        protected abstract void Apply(TTarget target, TValue value);

        protected virtual bool TryGetStyleValue(int styleIndex, out TValue value)
        {
            if (styleIndex < 0 || styleIndex >= styleValues.Length)
            {
                value = default;
                return false;
            }

            value = styleValues[styleIndex];
            return true;
        }

        public sealed override void Apply(int styleIndex)
        {
            if (target == null)
            {
                Debug.LogError($"Style target is null on '{name}'");
                return;
            }

            if (!TryGetStyleValue(styleIndex, out var value))
            {
                Debug.LogError($"No style with index '{styleIndex}' at '{name}'");
                return;
            }

            Apply(target, value);

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (target == null || target.gameObject != gameObject)
            {
                target = GetComponent<TTarget>();
            }
        }

        protected override void Reset()
        {
            target = GetComponent<TTarget>();
        }
    }

    public abstract class Style<TTarget, TValue, TAsset> : Style<TTarget, TValue>
        where TTarget : Component
        where TAsset : StyleAsset<TValue>
    {
        [SerializeField] private TAsset asset = default;

        public override string[] StyleNames => asset != null ? asset.StyleNames : base.StyleNames;

        protected override bool TryGetStyleValue(int styleIndex, out TValue value)
        {
            if (asset == null)
            {
                return base.TryGetStyleValue(styleIndex, out value);
            }

            if (styleIndex < 0 || styleIndex >= asset.StyleValues.Length)
            {
                value = default;
                return false;
            }

            value = asset.StyleValues[styleIndex];
            return true;
        }
    }
}