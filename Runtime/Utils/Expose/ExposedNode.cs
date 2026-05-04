using System.Collections.Generic;
using System.Reflection;

namespace LuckiusDev.Utils.Expose
{
    public class ExposedNode
    {
        public string Name;
        public object Value;
        public FieldInfo Field;
        public object Target;

        public List<ExposedNode> Children = new();

        public bool IsLeaf => Children.Count == 0;
    }
}