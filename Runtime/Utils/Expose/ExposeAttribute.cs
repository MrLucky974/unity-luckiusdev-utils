using System;

namespace LuckiusDev.Utils.Expose
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct)]
    public class ExposeAttribute : Attribute
    {
        public readonly string DisplayName;

        public ExposeAttribute(string displayName = null)
        {
            DisplayName = displayName;
        }
    }
}