using System;
using System.Globalization;
using Osram.DaliProControl.Core.ViewModels;
using Xamarin.Forms;

namespace Osram.DaliProControl.Core.Converters
{
    public class SecondTunningTypeToImageConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LightLevelSetupType)
            {
                var lightLevelSetupType = (LightLevelSetupType)value;

                switch (lightLevelSetupType)
                {
                    case LightLevelSetupType.LightAndColor:
                        return "colorcontrolicon.png";
                    case LightLevelSetupType.LightAndTemperature:
                        return "temperaturecontrolicon.png";
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}