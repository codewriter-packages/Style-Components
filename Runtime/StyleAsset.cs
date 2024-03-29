using System;
using UnityEngine;

namespace CodeWriter.StyleComponents
{
    public abstract class StyleAsset : ScriptableObject
    {
        public abstract Type ElementType { get; }

        public abstract string[] StyleNames { get; }
    }
}