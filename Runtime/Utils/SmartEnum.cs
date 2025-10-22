using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LuckiusDev.Utils
{
    public abstract class SmartEnum<T> where T : SmartEnum<T>
    {
        private static readonly Lazy<List<T>> s_lazyValues =
            new(() => typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(T))
                .Select(f => (T)f.GetValue(null))
                .ToList()
            );

        public static IReadOnlyList<T> Values => s_lazyValues.Value.AsReadOnly();

        // Common properties
        public string Name { get; }

        protected SmartEnum(string name)
        {
            Name = name;
        }

        // Parsing by name (case-insensitive)
        public static T FromName(string name)
        {
            return Values.FirstOrDefault(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                ?? throw new ArgumentException($"No {typeof(T).Name} found with name: {name}");
        }

        public override string ToString() => Name;

        public override bool Equals(object obj) => obj is T other && Name == other.Name;
        public override int GetHashCode() => Name.GetHashCode();
        
        public static bool operator ==(SmartEnum<T> left, SmartEnum<T> right) => Equals(left, right);
        public static bool operator !=(SmartEnum<T> left, SmartEnum<T> right) => !Equals(left, right);
    }
}
