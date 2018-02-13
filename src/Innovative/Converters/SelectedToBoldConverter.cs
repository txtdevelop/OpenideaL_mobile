using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using Osram.DaliProControl.Core.Resources;
using Xamarin.Forms;

namespace Osram.DaliProControl.Core.Converters
{
    public class SelectedToBoldConverter : MvxValueConverter<bool, FontAttributes>, IValueConverter
    {
        protected override FontAttributes Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? FontAttributes.Bold: FontAttributes.None;
        }
    }
}
