using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using Osram.DaliProControl.Core.Resources;
using Xamarin.Forms;

namespace Osram.DaliProControl.Core.Converters
{
    public class SelectedToColorConverter : MvxValueConverter<bool, Color>, IValueConverter
    {
        protected override Color Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? Styles.OsramColor: Styles.MainDarkTextColor;
        }
    }
}
