using UnityEngine;

namespace LuckiusDev.Utils.ScriptableObjects
{
    public abstract class BaseScriptableObject : ScriptableObject
    {
        [ScriptableObjectId] public string m_identifier;
    }
}