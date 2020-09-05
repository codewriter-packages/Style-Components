namespace CodeWriter.StyleComponents
{
    using UnityEngine;

    public abstract class StyleNamesSource : ScriptableObject
    {
        public abstract string[] Names { get; }
    }
}