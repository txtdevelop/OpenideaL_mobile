using System;
using MvvmCross.Platform;
using Xamarin.Forms;
using BindingFlags = System.Reflection.BindingFlags;
using UIKit;

namespace PSY.Innovative.iOS.Services
{
    public class TabGestureService
    {
        private UIView _currentViewPager;

        public void UpdateCurrentTabView(Page page)
        {

            var appDelegate = Mvx.Resolve<AppDelegate>();
            var viewGroup = appDelegate.Window.RootViewController.View.Subviews[0];

            _currentViewPager = FindChildOfType<UIPageControl>(viewGroup);
        }

        public void SetGestureEnabled(bool enabled)
        {
            if (_currentViewPager == null) return;
            var type = _currentViewPager.GetType();
            type.InvokeMember("EnableGesture",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, _currentViewPager, new object[] { enabled });
        }

        private static UIView FindChildOfType<T>(UIView parent)
        {
            if (parent.Subviews.Length == 0)
                return null;

            for (var i = 0; i < parent.Subviews.Length; i++)
            {
                var child = parent.Subviews[i];

                if (child == null)
                    continue;

                if (child is T)
                {
                    return child;
                }

                var result = FindChildOfType<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}