using UnityEngine;

namespace LuckiusDev.Utils
{
    /// <summary>
    /// Read Only attribute.
    /// Attribute is use only to mark ReadOnly properties.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public bool OnPlayMode { get; }

        public ReadOnlyAttribute(bool onPlayMode = false)
        {
            OnPlayMode = onPlayMode;
        }
    }
}