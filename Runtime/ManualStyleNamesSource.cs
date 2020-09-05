namespace CodeWriter.StyleComponents
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Style Names Source", menuName = "Style Components/Names Source")]
    public class ManualStyleNamesSource : StyleNamesSource
    {
        [SerializeField] private string[] names = new string[0];

        public override string[] Names => names;
    }
}