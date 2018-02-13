using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using Osram.DaliProControl.Core.Resources;
using Xamarin.Forms;

namespace Osram.DaliProControl.Core.Converters
{
    public class BoolToStringConverter: MvxValueConverter<bool, string>, IValueConverter
    {
        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? AppResources.Yes: AppResources.No;
        }
    }
}
