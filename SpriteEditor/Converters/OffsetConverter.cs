using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using static System.Convert;

namespace SpriteEditor.Converters
{
    public class OffsetConverter : IMultiValueConverter
    {
        public Point LastInitialPoint { get; private set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            LastInitialPoint = (Point)values[0];
            var offset = ToInt32(values[1]);

            var type = parameter.ToString();
            return type switch
            {
                "X" => LastInitialPoint.X + offset,
                "Y" => LastInitialPoint.Y + offset,
                _ => throw new ArgumentException()
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var val = ToInt32(value);
            var type = parameter.ToString();
            var offset = type switch
            {
                "X" => val - LastInitialPoint.X,
                "Y" => val - LastInitialPoint.Y,
                _ => throw new ArgumentException()
            };
            return new object[]
            {
                LastInitialPoint,
                ToInt32(offset)
            };
        }
    }
}
