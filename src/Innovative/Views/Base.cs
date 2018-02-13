using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform;
using SlideOverKit;
using PSY.Innovative.Controls;
using PSY.Innovative.Helpers;
using PSY.Innovative.ViewModels;
using PSY.Innovative.ViewPresenter;
using Xamarin.Forms;

namespace PSY.Innovative.Views
{
    public class Base : MenuContainerPage
    {
        public Base()
        {
            SetBinding(TitleProperty, new Binding("Title"));
            BackgroundColor = Color.FromRgb(240, 240, 240);
        }
        ~Base()
        {
            Logger.Trace("GC'ed view {0}: ", GetType().Name);
        }
        public bool IsTab { get; set; }

        public virtual void OnInitialized()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    foreach (var toolbarItem in AndroidToolbarItems)
                    {
                        ToolbarItems.Add(toolbarItem);
                    }
                    break;
                case Device.iOS:
                    foreach (var toolbarItem in IosToolbarItems)
                    {
                        ToolbarItems.Add(toolbarItem);
                    }
                    break;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as BaseViewModel;

            vm?.Resumed();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var vm = BindingContext as BaseViewModel;
            vm?.Loaded();

            //if (vm is ConnectionBaseViewModel)
            //{
            //    AddConnectionStatusIndicator();
            //}
        }

        //private void AddConnectionStatusIndicator()
        //{
        //    var grid = Content as Grid;
        //    if (grid == null) return;

        //    var typeEqual = grid.RowDefinitions[0].Height.IsAbsolute ==
        //                    Styles.ConnectionStatusIndicatorHeight.IsAbsolute;
        //    var valueEqual = (int)grid.RowDefinitions[0].Height.Value ==
        //                     (int)Styles.ConnectionStatusIndicatorHeight.Value;

        //    if (!typeEqual || !valueEqual)
        //    {
        //        Logger.Trace("Grid row height not fit for indicator...skipping. There should be one row on top with Height=Styles.ConnectionStatusIndicatorHeight");
        //        return;
        //    }

        //    var connectionIndicator = CreateConnectionStatusIndicator();

        //    grid.Children.Add(connectionIndicator, 0, 0);
        //}

        protected virtual ConnectionStatusIndicator CreateConnectionStatusIndicator()
        {
            var connectionIndicator = new ConnectionStatusIndicator();

            connectionIndicator.SetBinding(ConnectionStatusIndicator.CurrentDeviceStateProperty,
                new Binding("CurrentDeviceState"));

            return connectionIndicator;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var vm = BindingContext as BaseViewModel;
            if (vm == null) return;

            //TODo refactor this platform specific cleaner
            var isInStack = Navigation.NavigationStack.Contains(this);

            var isNotTop = Navigation.NavigationStack.LastOrDefault() != this || Mvx.Resolve<MasterDetailViewPresenter>().IsShowingModalView;

            // prevent cleanup in case a modal view si showing over our view... 
            //(apparently this is not even in the stack at htis point, so wee need to get this from the view presenter)

            if (Innovative.AppState == AppState.Suspended || IsTab || (isInStack && isNotTop))
            {
                vm.Suspended();
                return;
            }

            vm.Suspended();
            vm.Cleanup();
        }

        public IList<ToolbarItem> AndroidToolbarItems { get; set; } = new List<ToolbarItem>();


        public IList<ToolbarItem> IosToolbarItems { get; set; } = new List<ToolbarItem>();

        public bool ShowModalCloseButton { get; set; }
    }
}
