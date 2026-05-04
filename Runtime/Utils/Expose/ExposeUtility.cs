using System.Collections.Generic;
using System.Reflection;

namespace LuckiusDev.Utils.Expose
{
    public static class ExposeUtility
    {
        public static List<ExposedField> GetExposedFields(object obj)
        {
            var result = new List<ExposedField>();
            if (obj == null) return result;

            var type = obj.GetType();

            // Only traverse types marked with [Expose]
            if (!HasExpose(type)) return result;

            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var field in type.GetFields(flags))
            {
                var attr = field.GetCustomAttribute<ExposeAttribute>();
                if (attr == null) continue;

                var value = field.GetValue(obj);

                result.Add(new ExposedField
                {
                    Target = obj,
                    Field = field,
                    DisplayName = attr.DisplayName ?? field.Name
                });

                // Recurse into nested exposed types
                if (value != null && HasExpose(field.FieldType))
                {
                    result.AddRange(GetExposedFields(value));
                }
            }

            return result;
        }

        private static bool HasExpose(System.Type type)
        {
            return type.GetCustomAttribute<ExposeAttribute>() != null;
        }
    }
}