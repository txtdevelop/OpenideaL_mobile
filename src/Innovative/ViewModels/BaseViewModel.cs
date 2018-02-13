using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using PSY.Innovative.Contracts;
using PSY.Innovative.Helpers;
using PSY.Innovative.Utils;
using PSY.Innovative.ViewPresenter;

namespace PSY.Innovative.ViewModels
{
    /// <summary>
    ///     Navigation hints for vm requests
    /// </summary>
    public enum RequestType
    {
        Root,
        Modal,
        Clean,
        None,
        PreAppStart,
        MasterDetail
    }

    /// <summary>
    ///     Defines the BaseViewModel type. Adds some usefull VM lifecycle methods
    /// </summary>
    public abstract class BaseViewModel : MvxViewModel
    {
        //Key for bundle request args
        public const string RequestTypeKey = "RequestType";

        //TODO: Replace this static field with an injectable class
        public static readonly Dictionary<string, object> MemoryCache = new Dictionary<string, object>();

        /// <summary>
        ///     Is Busy property for various view busy indicators and command execute.
        /// </summary>
        private bool _isBusy;

        /// <summary>
        ///     Page title
        /// </summary>
        private string _title = string.Empty;
//        protected readonly IEventAggregatorComponent EventAggregator;

        protected BaseViewModel()
        {
            //TODO: Inject ILocalizationService from constructor parameters
//            EventAggregator = Mvx.Resolve<IEventAggregatorComponent>();

            LocalizationService = GetService<ILocalizationService>();
//            EventAggregator.Subscribe<Events.OnLanguageChangedEvent>(OnLanguageChanged);

            CancelCommand = new MvxCommand(Cancel);
        }
        public ICommand CancelCommand { get; }

        private void OnLanguageChanged()
        {
            RaiseAllPropertiesChanged();
        }

        public virtual string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        public virtual bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        protected virtual BaseViewModelParameter ClassParameter { get; set; }

        /// <summary>
        ///     Gets the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>An instance of the service.</returns>
        public TService GetService<TService>() where TService : class
        {
            //TODO: Remove this method and use dependency injection in derived classes instead
            return Mvx.Resolve<TService>();
        }

        /// <summary>
        ///     Checks if a property already matches a desired value.  Sets the property and
        ///     notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="backingStore">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="property">The property.</param>
        protected void SetProperty<T>(ref T backingStore, T value, Expression<Func<T>> property)
        {
            if (Equals(backingStore, value))
            {
                return;
            }

            backingStore = value;

            RaisePropertyChanged(property);
        }


        /// <summary>
        ///     Shows the view model as the root of the master detail page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ShowAsRootViewModel<T>(BaseViewModelParameter parameter = null) where T : MvxViewModel
        {
            ShowViewModel<T>(RequestType.Root, parameter);
        }

        /// <summary>
        ///     Shows the view model after cleaning the master detail navigation stack back to the root (home)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ShowAsCleanViewModel<T>(BaseViewModelParameter parameter = null) where T : MvxViewModel
        {
            ShowViewModel<T>(RequestType.Clean, parameter);
        }

        /// <summary>
        ///     Tries to show the view model in the current navigation stack
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ShowModalViewModel<T>(BaseViewModelParameter parameter = null) where T : MvxViewModel
        {
            ShowModalViewModel(typeof(T), parameter);
        }

        /// <summary>
        ///     Shows the view modal modally and creates a new navigations tack for the modal view
        /// </summary>
        /// <param name="viewModelType"></param>
        public void ShowModalViewModel(Type viewModelType, BaseViewModelParameter parameter = null)
        {
            AddParameterToCache(viewModelType, parameter);

            ShowViewModel(viewModelType, presentationBundle: CreateBundleForRequestType(RequestType.Modal));
        }

        private static void AddParameterToCache(Type viewModelType, BaseViewModelParameter parameter)
        {
            if (parameter != null)
            {
                MemoryCache[viewModelType.Name] = parameter;
            }
        }

        public void ShowViewModel<T>(BaseViewModelParameter parameter = null) where T : MvxViewModel
        {
            AddParameterToCache(typeof(T), parameter);

            base.ShowViewModel<T>();
        }


        private void ShowViewModel<T>(RequestType specialRequest, BaseViewModelParameter parameter = null) where T : MvxViewModel
        {
            AddParameterToCache(typeof(T), parameter);

            ShowViewModel<T>(presentationBundle: CreateBundleForRequestType(specialRequest));
        }

        public void MyShowViewModel<T>(RequestType specialRequest, BaseViewModelParameter parameter = null) where T : MvxViewModel
        {
            AddParameterToCache(typeof(T), parameter);

            ShowViewModel<T>(presentationBundle: CreateBundleForRequestType(specialRequest));
        }

        protected void GoBackHome()
        {
            ChangePresentation(new GoBackToHomePresentationHint());
        }

        private static MvxBundle CreateBundleForRequestType(RequestType specialRequest)
        {
            return new MvxBundle(new Dictionary<string, string> { { RequestTypeKey, specialRequest.ToString() } });
        }

        protected virtual void Cancel()
        {
            //clean cache
            MemoryCache.Remove(SettingsKeys.ParameterCacheKey);

            ChangePresentation(new CloseModalPresentationHint(GetType()));
        }


        protected ILocalizationService LocalizationService { get; private set; }


        #region Lifecycle
        public virtual bool IsShown { get; protected set; }

        /// <summary>
        ///     This method is called whenever a view is popped form the navigation stack for the view before it
        /// </summary>
        public virtual void Resumed()
        {
            IsShown = true;
#if DEBUG
            Logger.Trace($"VM resumed: {GetType().Name}");
#endif
        }

        /// <summary>
        ///     This method is called whenever we navigate from a veiw to another (push something to the navigation stack)
        /// </summary>
        public virtual void Suspended()
        {
            IsShown = false;
#if DEBUG
            Logger.Trace($"VM suspended: {GetType().Name}");
#endif
        }

        public virtual Task Cleanup()
        {
            IsShown = false;
#if DEBUG
            Logger.Trace($"VM cleaned up: {GetType().Name}");
#endif
            return Task.FromResult(true);
        }

        /// <summary>
        ///     Called the first time the view is shown
        /// </summary>
        public virtual void Loaded()
        {
            IsShown = true;
#if DEBUG
            Logger.Trace($"VM loaded: {GetType().Name}");
#endif
        }

        public virtual void Close()
        {
            Close(this);
#if DEBUG
            Logger.Trace($"VM closed: {GetType().Name}");
#endif
        }


        public bool IsInDebug =>
#if DEBUG 
            true;
#else
        false;
#endif


        public override void Start()
        {
            base.Start();
#if DEBUG
            Logger.Trace($"VM started: {GetType().Name}");
#endif
            object param;
            if (MemoryCache.TryGetValue(GetType().Name, out param) &&
                param is BaseViewModelParameter)
            {
                ClassParameter = (BaseViewModelParameter)param;
                MemoryCache.Remove(GetType().Name);
            }
        }

        #endregion
    }
}