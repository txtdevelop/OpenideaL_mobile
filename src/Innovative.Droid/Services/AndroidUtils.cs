using System;
using Android.OS;
using Android.Views;
using MvvmCross.Platform;
using PSY.Innovative.Contracts;

namespace PSY.Innovative.Droid.Services
{
    public class AndroidUtils : IAndroidUtils, Contracts.ISystemUtils
    {
        public void CloseApp()
        {
            Process.KillProcess(Process.MyPid());
        }

        public void PreventDeviceSleep(bool shouldPrevent)
        {
            try
            {
                if (shouldPrevent)
                {
                    Mvx.Resolve<MainActivity>().Window.AddFlags(WindowManagerFlags.KeepScreenOn);
                }
                else
                {
                    Mvx.Resolve<MainActivity>().Window.ClearFlags(WindowManagerFlags.KeepScreenOn);
                }
            }
            catch (Exception ex)
            {
                ex.TrackWarning(GetType().Name, "PreventDeviceSleep");
            }
        }
    }
}