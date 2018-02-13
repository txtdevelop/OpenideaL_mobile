using System;
using Android.Support.V4.View;
using Android.Views;
using MvvmCross.Platform;
using Xamarin.Forms;
using BindingFlags = System.Reflection.BindingFlags;

namespace PSY.Innovative.Droid.Services
{
    public class TabGestureService
    {
        private ViewGroup _currentViewPager;

        public void UpdateCurrentTabView(Page page)
        {
            if (!(page is TabbedPage)) return;

            var main = Mvx.Resolve<MainActivity>();
            var viewGroup = (ViewGroup) ((ViewGroup) main.FindViewById(Android.Resource.Id.Content)).GetChildAt(0);

            _currentViewPager = FindChildOfType<ViewPager>(viewGroup);
        }

        public void SetGestureEnabled(bool enabled)
        {
            if (_currentViewPager == null) return;
            var type = _currentViewPager.GetType();
            type.InvokeMember("EnableGesture",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
                Type.DefaultBinder, _currentViewPager, new object[] {enabled});
        }

        private static ViewGroup FindChildOfType<T>(ViewGroup parent)
        {
            if (parent.ChildCount == 0)
                return null;

            for (var i = 0; i < parent.ChildCount; i++)
            {
                var child = parent.GetChildAt(i) as ViewGroup;

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