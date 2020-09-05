namespace CodeWriter.StyleComponents
{
    using System;
    using UnityEngine;

    public class StyleContext : MonoBehaviour
    {
        [SerializeField] private Variable[] variables = new Variable[0];

        [Serializable]
        private class Variable
        {
            public string key;
            public string defaultValue;
        }

        private string[] _variablesArray;

        public string[] VariablesArray
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    return CreateVariablesArray(variables);
                }
#endif

                return _variablesArray;
            }
        }

        private void Awake()
        {
            _variablesArray = CreateVariablesArray(variables);
        }

        private static string[] CreateVariablesArray(Variable[] variables)
        {
            var array = new string[variables.Length * 2];

            int index = 0;
            foreach (var variable in variables)
            {
                array[index++] = variable.key;
                array[index++] = variable.defaultValue;
            }

            return array;
        }

        public void SetVariable(string key, string value)
        {
            for (var i = 0; i < _variablesArray.Length; i += 2)
            {
                if (_variablesArray[i].Equals(key, StringComparison.InvariantCulture))
                {
                    _variablesArray[i + 1] = value;
                    return;
                }
            }

            var obj = gameObject;
            Debug.LogError($"Key {key} not exists at {obj.name}", obj);
        }
    }
}