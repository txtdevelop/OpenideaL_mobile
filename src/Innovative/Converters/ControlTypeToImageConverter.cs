using System;
using System.Globalization;
using Osram.DaliProControl.Core.ViewModels;
using Xamarin.Forms;

namespace Osram.DaliProControl.Core.Converters
{
    public class ControlTypeToImageConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ControlType)
            {
                var controlType = (ControlType) value;

                switch (controlType)
                {
                    case ControlType.LightLevel:
                        return "lightcontrolicon.png";
                    case ControlType.ColorLevel:
                        return "colorcontrolicon.png";
                    case ControlType.TemperatureLevel:
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