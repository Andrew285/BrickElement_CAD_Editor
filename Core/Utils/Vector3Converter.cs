using System.ComponentModel;
using System.Globalization;
using System.Numerics;

namespace Core.Utils
{
    public class Vector3Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string strValue)
            {
                if (strValue.Contains('.'))
                {
                    strValue = strValue.Replace('.', ',');
                }
                string[] parts = strValue.Split(", ");
                if (parts.Length == 3 &&
                    float.TryParse(parts[0], out float x) &&
                    float.TryParse(parts[1], out float y) &&
                    float.TryParse(parts[2], out float z))
                {
                    return new Vector3(x, y, z);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Vector3 vec)
            {
                return $"{vec.X}, {vec.Y}, {vec.Z}";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
