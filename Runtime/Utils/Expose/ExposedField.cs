using System.Reflection;

namespace LuckiusDev.Utils.Expose
{
    public class ExposedField
    {
        public object Target;
        public FieldInfo Field;
        public string DisplayName;

        public object GetValue()
        {
            return Field.GetValue(Target);
        }

        public void SetValue(object value)
        {
            Field.SetValue(Target, value);
        }
    }
}