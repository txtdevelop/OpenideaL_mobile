using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using Xamarin.Forms;

namespace Osram.DaliProControl.Core.Converters
{
    public class IntToBoolConverter : MvxValueConverter<int, bool>, IValueConverter
    {
        protected override bool Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return value > 0;
        }
    }
}