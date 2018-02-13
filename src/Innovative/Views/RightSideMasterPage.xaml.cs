using SlideOverKit;
using Xamarin.Forms;

namespace PSY.Innovative.Views
{
    public partial class RightSideMasterPage : SlideMenuView
    {
//        private AreasMenu _menu;

        public RightSideMasterPage ()
        {
            //_menu = new AreasMenu();

//            Content = _menu;

            // You must set IsFullScreen in this case, 
            // otherwise you need to set HeightRequest, 
            // just like the QuickInnerMenu sample
            this.IsFullScreen = true;
            // You must set WidthRequest in this case
            this.WidthRequest = 250;
            this.MenuOrientations = MenuOrientation.RightToLeft;

            // You must set BackgroundColor, 
            // and you cannot put another layout with background color cover the whole View
            // otherwise, it cannot be dragged on Android
           // this.BackgroundColor = Color.White;

            // This is shadow view color, you can set a transparent color
            this.BackgroundViewColor = Styles.PsyDarkColor;//Color.FromRgb(0xE9, 0x5B, 0x0D)
            this.BackgroundViewColor = Color.FromRgba(0x00, 0x00, 0x00, 0x66);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

//           _menu.BindingContext = BindingContext;
        }
    }
}

