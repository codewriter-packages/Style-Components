namespace CodeWriter.StyleComponents
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;

    public class StyledButtonMenu
    {
        [MenuItem("CONTEXT/Button/Convert To Styled Button")]
        private static void ConvertToStyledButton(MenuCommand command)
        {
            EditorUtilities.ConvertScriptTo<StyledButton>(command.context as MonoBehaviour);
        }

        [MenuItem("CONTEXT/StyledButton/Convert To Button")]
        private static void ConvertToButton(MenuCommand command)
        {
            EditorUtilities.ConvertScriptTo<Button>(command.context as MonoBehaviour);
        }
    }
}