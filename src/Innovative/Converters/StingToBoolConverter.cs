using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using Xamarin.Forms;

namespace Osram.DaliProControl.Core.Converters
{
    public class StingToBoolConverter : MvxValueConverter<string, bool>, IValueConverter
    {
        protected override bool Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return !String.IsNullOrEmpty(value);
        }
    }
}