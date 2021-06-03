#if TEXT_MESH_PRO

namespace CodeWriter.StyleComponents
{
    using TMPro;
    using UnityEngine;
    using StyleAssets;
    using ViewBinding;

    [RequireComponent(typeof(TMP_Text))]
    [AddComponentMenu("Style Components/Tmp Text Style")]
    public sealed class TmpTextStyle : Style<TMP_Text, string, TextStyleAsset>, IViewContextListener
    {
        private TMP_Text _lastTarget;
        private string _lastValue;

        protected override void Apply(TMP_Text target, string value)
        {
            _lastTarget = target;
            _lastValue = value;

            if (Context != null)
            {
                target.SetText(Context.FormatText(value));
            }
            else
            {
                target.text = value;
            }
        }

        protected override void Start()
        {
            base.Start();

            ReSubScribe();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            ReSubScribe();
        }

        private void ReSubScribe()
        {
            if (Context != null)
            {
                Context.AddListener(this);
            }
        }

        public void OnContextVariableChanged(ViewVariable variable)
        {
            if (_lastTarget != null)
            {
                Apply(_lastTarget, _lastValue);
            }
        }
    }
}
#endif