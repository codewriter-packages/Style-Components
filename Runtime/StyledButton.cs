using UnityEngine;

namespace CodeWriter.StyleComponents
{
    using UnityEngine.UI;

    [RequireComponent(typeof(Style))]
    public class StyledButton : Button
    {
        private Style _style = default;

        protected override void Awake()
        {
            base.Awake();

            _style = GetComponent<Style>();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (!gameObject.activeInHierarchy)
                return;

            if (!_style)
            {
                return;
            }

            _style.Apply(interactable ? "Normal" : "Disabled");
        }
    }
}