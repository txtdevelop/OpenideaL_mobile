using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using Xamarin.Forms;

namespace PSY.Innovative.Converters
{
    public class BoolInvertConverter : MvxValueConverter<bool, bool>, IValueConverter
    {
        protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return !value;
        }
    }
}