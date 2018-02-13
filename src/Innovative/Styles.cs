using Xamarin.Forms;

namespace PSY.Innovative
{
    public static class Styles
    {
        public static readonly Color PsyDarkColor = Color.FromRgb(0x41, 0xB6, 0x59);
        public static readonly Color PsyLightColor = Color.FromRgb(0xA7, 0xD5, 0xA2);

        public static readonly Color MainDarkTextColor = Color.FromRgba(0, 0, 0, 0.87);
        public static readonly Color SecondaryDarkTextColor = Color.FromRgba(0, 0, 0, 0.54);

        public static readonly Color MainMenuBackGround = Color.White;

        public static readonly Thickness CellContentPadding = Device.RuntimePlatform == Device.Android
            ? new Thickness(5)
            : new Thickness(10, 5, 5, 5);
        
    }
}