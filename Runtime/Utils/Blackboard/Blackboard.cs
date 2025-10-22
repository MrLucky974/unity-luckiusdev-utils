using System;
using System.Collections.Generic;

namespace LuckiusDev.Utils.Blackboard
{
    public class Blackboard
    {
        private readonly Dictionary<Enum, object> m_values = new();

        public void Set<TEnum, TValue>(TEnum key, TValue value) where TEnum : Enum
        {
            m_values[key] = value;
        }

        public TValue Get<TEnum, TValue>(TEnum key) where TEnum : Enum
        {
            if (!m_values.TryGetValue(key, out var value))
                throw new ArgumentException($"Could not find value for key: {key} in values.");
            return (TValue)value;
        }

        public bool TryGet<TEnum, TValue>(TEnum key, out TValue value, TValue defaultValue = default) where TEnum : Enum
        {
            if (m_values.TryGetValue(key, out var value1))
            {
                value = (TValue)value1;
                return true;
            }

            value = defaultValue;
            return false;
        }
    }
}
