using UnityEngine;

namespace LuckiusDev.Utils
{
    public abstract class BaseScriptableObject : ScriptableObject
    {
        [ScriptableObjectId] private string m_identifier;
        public string Identifier => m_identifier;
    }
}