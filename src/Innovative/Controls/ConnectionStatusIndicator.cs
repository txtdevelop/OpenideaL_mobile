using System;
using System.Threading.Tasks;
using PSY.Innovative.ViewModels;
using Xamarin.Forms;

namespace PSY.Innovative.Controls
{
    public class ConnectionStatusIndicator : ContentView
    {
        public static readonly BindableProperty CurrentDeviceStateProperty =
            BindableProperty.Create(nameof(CurrentDeviceState), typeof(CurrentDeviceState), typeof(ConnectionStatusIndicator),
                CurrentDeviceState.Offline, propertyChanged: ((bindable, value,
                    newValue) =>
                {
                    var control = bindable as ConnectionStatusIndicator;

                    if (control != null)
                    {
                        control.UpdateColor((CurrentDeviceState)newValue);
                    }
                }));

        public static readonly BindableProperty WorkingProgressProperty =
            BindableProperty.Create(nameof(WorkingProgressActive), typeof(bool), typeof(ConnectionStatusIndicator), false,
                propertyChanged: ((bindable, value,
                    newValue) =>
                {
                    var control = bindable as ConnectionStatusIndicator;

                    if (control != null)
                    {
                        if ((bool)newValue)
                        {
                            control.StartWorkingProgress();
                        }
                        else
                        {
                            control.StopWorkingProgress();
                        }
                    }
                }));

        private readonly BoxView _statusIndicator;
        private Task _animationTask;
        private bool _isAnimating;
        //private CancellationTokenSource _tokenSource;

        public ConnectionStatusIndicator()
        {
            HorizontalOptions = LayoutOptions.Fill;

            _statusIndicator = new BoxView
            {
                IsVisible = false
            };

            var absoluteLayout = new AbsoluteLayout();
            absoluteLayout.Children.Add(_statusIndicator, new Rectangle(0, 0, 50, 3));

            Content = absoluteLayout;
        }

        public CurrentDeviceState CurrentDeviceState
        {
            get { return (CurrentDeviceState)GetValue(CurrentDeviceStateProperty); }
            set { SetValue(CurrentDeviceStateProperty, value); }
        }


        public bool WorkingProgressActive
        {
            get { return (bool)GetValue(WorkingProgressProperty); }
            set { SetValue(WorkingProgressProperty, value); }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateColor(CurrentDeviceState);
        }


        private void UpdateColor(CurrentDeviceState state)
        {
            switch (state)
            {
                case CurrentDeviceState.Offline:
                    BackgroundColor = Color.Red;
                    _statusIndicator.Color = Color.FromRgb(255, 255, 255).MultiplyAlpha(0.8);
                    break;
                case CurrentDeviceState.Online:
                    BackgroundColor = Color.FromRgb(0x20, 0xBD, 0x42);
                    _statusIndicator.Color = Color.FromRgb(255, 255, 255).MultiplyAlpha(0.5);
                    break;
            }
        }

        private async void StartWorkingProgress()
        {
            await EnsureAnimationStopped();

            Device.BeginInvokeOnMainThread(AnimateScan);
        }

        private async void StopWorkingProgress()
        {
            await EnsureAnimationStopped();
        }

        private async Task EnsureAnimationStopped()
        {
            try
            {
                _isAnimating = false;
                if (_animationTask != null)
                    await _animationTask;
            }
            catch (Exception ex)
            {
                ex.TrackWarning(GetType().Name, "EnsureAnimationStopped");
            }
        }

        private async void AnimateScan()
        {
            try
            {
                if (_isAnimating)
                    return;

                _isAnimating = true;

                var directionRight = true;

                _statusIndicator.IsVisible = true;


                while (_isAnimating)
                {
                    if (directionRight)
                    {
                        _animationTask =
                            _statusIndicator.LayoutTo(
                                new Rectangle(Width - _statusIndicator.Width, 0, _statusIndicator.Width, 3), 1000U);
                        await _animationTask.ConfigureAwait(true);

                        directionRight = false;
                    }
                    else
                    {
                        _animationTask = _statusIndicator.LayoutTo(new Rectangle(0, 0, _statusIndicator.Width, 3), 1000U);
                        await _animationTask.ConfigureAwait(true);

                        directionRight = true;
                    }
                }

                _statusIndicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                ex.TrackWarning(GetType().Name, "AnimateScan");
                _isAnimating = false;
            }
        }
    }
}