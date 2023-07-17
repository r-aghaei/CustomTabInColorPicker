using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace CustomTabInColorPicker
{
    public class CustomColorConverter : ColorConverter
    {
        Dictionary<int, string> colorNames = new Dictionary<int, string>
    {
        {Color.Red.ToArgb(), "Blood"},
        {Color.Green.ToArgb(), "Life potion"},
        {Color.Blue.ToArgb(), "Water"},
    };
        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) &&
                value is Color &&
                colorNames.ContainsKey(((Color)value).ToArgb()))
                return colorNames[((Color)value).ToArgb()];
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                foreach (var item in colorNames)
                {
                    if (string.Equals(item.Value, (string)value,
                        StringComparison.OrdinalIgnoreCase))
                        return Color.FromArgb(item.Key);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
