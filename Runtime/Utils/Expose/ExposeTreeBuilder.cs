using System.Collections;
using System.Reflection;

namespace LuckiusDev.Utils.Expose
{
    public static class ExposeTreeBuilder
    {
        private static BindingFlags FLAGS =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static ExposedNode Build(object obj, string name = null)
        {
            if (obj == null) return null;

            var type = obj.GetType();

            if (!HasExpose(type))
                return null;

            var node = new ExposedNode
            {
                Name = name ?? type.Name,
                Value = obj,
                Target = obj
            };

            foreach (var field in type.GetFields(FLAGS))
            {
                var attr = field.GetCustomAttribute<ExposeAttribute>();
                if (attr == null) continue;

                var value = field.GetValue(obj);

                var childName = attr.DisplayName ?? field.Name;

                var childNode = new ExposedNode
                {
                    Name = childName,
                    Value = value,
                    Field = field,
                    Target = obj
                };

                BuildChildren(childNode, value);

                node.Children.Add(childNode);
            }

            return node;
        }

        private static void BuildChildren(ExposedNode parent, object value)
        {
            if (value == null) return;

            var type = value.GetType();

            // COLLECTION SUPPORT
            if (typeof(IList).IsAssignableFrom(type))
            {
                var list = (IList)value;

                for (int i = 0; i < list.Count; i++)
                {
                    var element = list[i];
                    if (element == null) continue;

                    if (!HasExpose(element.GetType())) continue;

                    var elementNode = Build(element, $"Element {i}");
                    if (elementNode != null)
                        parent.Children.Add(elementNode);
                }

                return;
            }

            // NESTED OBJECT SUPPORT
            if (HasExpose(type))
            {
                var nested = Build(value, parent.Name);
                if (nested != null)
                    parent.Children.AddRange(nested.Children);
            }
        }

        public static bool HasExpose(System.Type type)
        {
            return type.GetCustomAttribute<ExposeAttribute>() != null;
        }
    }
}