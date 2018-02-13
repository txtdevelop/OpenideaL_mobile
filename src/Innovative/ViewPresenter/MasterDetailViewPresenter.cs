using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using PSY.Innovative.Helpers;
using PSY.Innovative.ViewModels;
using PSY.Innovative.Views;
using Xamarin.Forms;
using MvvmCross.Forms.Presenters;
using MvvmCross.Forms.Core;

namespace PSY.Innovative.ViewPresenter
{
    /// <summary>
    ///     Responsible for the entire navigation concept of the app (does NOT include tab view)
    ///     *First start navigation
    ///     *MasterDetail navigation
    ///     *Detail navigation
    /// </summary>
    public class MasterDetailViewPresenter : MvxFormsPagePresenter
    {
        protected virtual void OnPagePresented(Page page)
        {
        }

        public MasterDetailPage RootMasterDetailPage { get; private set; }

        public INavigation MainPageNavigation => MvxFormsApplication.Current.MainPage.Navigation;

        private bool _isBusy;
        private bool _isBusyClosing;

        public bool IsShowingModalView { get; private set; }

        #region C'tors

        //
        // Constructors
        //
        public MasterDetailViewPresenter()
        {
        }

        public MasterDetailViewPresenter(MvxFormsApplication mvxFormsApplication) : base(mvxFormsApplication)
        {
        }

        #endregion

        #region Overrides


        public ViewModelPresentationType GetViewModelPresentationType(Type viewModelType)
        {
            var viewModelPresentationAttribute = GetCustomAttribute<ViewModelPresentationAttribute>(viewModelType);
            if (viewModelPresentationAttribute != null)
            {
                return viewModelPresentationAttribute.PresentationType;
            }

            // default allow multiple instances
            return ViewModelPresentationType.AllowMultipleInstances;
        }

        private static TAttribute GetCustomAttribute<TAttribute>(Type type)
            where TAttribute : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() as TAttribute;
        }

        public override async void Show(MvxViewModelRequest request)
        {

            var presentationType = GetViewModelPresentationType(request.ViewModelType);
            if (presentationType == ViewModelPresentationType.AllowSingleInstanceOnTop)
            {
                var topmostViewModel = TryGetTopmostViewModel();
                if (topmostViewModel != null && topmostViewModel.GetType() == request.ViewModelType)
                {
                    // already on top
                    Logger.Error("Request refused for {0}. Presentation attribute set to: {1}", request.ViewModelType.Name, presentationType);
                    return;
                }
            }

            if (!await TryShowAsync(request))
            {
                Logger.Error("Skipping request for {0}", request.ViewModelType.Name);
            }
        }

        public override async void ChangePresentation(MvxPresentationHint hint)
        {
            try
            {
                if (HandlePresentationChange(hint))
                    return;

                if (MvxFormsApplication.Current.MainPage == null)
                {
                    return;
                }

                //ToDo figure this out, make Cancel awaitable :P
                // this will prevent closing multiple modal views quickly one after the other
                //if (_isBusyClosing)
                //   return;

                _isBusyClosing = true;

                // handle normal close
                if (hint is GoBackToHomePresentationHint)
                {
                    await HandleGoBackToHOmePresentationHint();
                }
                else if (hint is CloseModalPresentationHint)
                {
                    await HandleCloseModalPresentationHint((CloseModalPresentationHint)hint);
                }
                else if (hint is MvxClosePresentationHint)
                {
                    await HandleClosePesentationHint();
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(nameof(ChangePresentation), ex);
            }
            finally
            {
                _isBusyClosing = false;
            }
        }

        #endregion

        #region Handle change presentation (close)

        private async Task HandleClosePesentationHint()
        {
            // get the base page (either root or detail)
            var mainPage = (RootMasterDetailPage?.Detail ?? MvxFormsApplication.Current.MainPage) as NavigationPage;

            // get the topmost modal page
            var lastNavigationPage = MainPageNavigation.ModalStack.LastOrDefault() as NavigationPage;
            var pageToActivate = lastNavigationPage ?? mainPage;

            if (pageToActivate == null)
            {
                Logger.Trace($"{nameof(MasterDetailViewPresenter)}::{nameof(HandleClosePesentationHint)}", "No valid navigation page found");
                return;
            }

            await pageToActivate.PopAsync();
        }

        private async Task HandleCloseModalPresentationHint(CloseModalPresentationHint hint)
        {
            var navigation = MainPageNavigation;

            // this can not be popped modally
            if (!navigation.ModalStack.Any() || navigation.ModalStack.Last() is MasterDetailPage)
            {
                var topVm = TryGetTopmostViewModel();

                if (hint.ViewModelToCloseType == topVm.GetType())
                {
                    ChangePresentation(new MvxClosePresentationHint(null));
                }
                else
                {
                    throw new Exception("Failed to close modal view. No valid modal views on the stack. Also no vm of type found on top.");
                }
                return;
            }

            //modal pop
            var page = await navigation.PopModalAsync();
            Logger.Trace("Popped {0}", page.GetType().Name);
        }

        private async Task HandleGoBackToHOmePresentationHint()
        {
            var navigation = MainPageNavigation;
            var page = navigation.ModalStack?.LastOrDefault();

            var animated = true;
            while (page != null && !(page is MasterDetailPage))
            {
                await navigation.PopModalAsync(animated);
                animated = false;
                page = navigation.ModalStack.LastOrDefault();
            }

            await RootMasterDetailPage.Detail.Navigation.PopToRootAsync(animated);
        }

        #endregion

        #region Handle navigation requests

        public RequestType GetRequestType(MvxViewModelRequest request)
        {
            var requestType = RequestType.None;
            if (request.PresentationValues != null)
            {
                var serializedRequestType = request.PresentationValues[BaseViewModel.RequestTypeKey];
                Enum.TryParse(serializedRequestType, true, out requestType);
            }

            if (request.ViewModelType == typeof(MasterDetailViewModel))
                return RequestType.MasterDetail;

            //override request type if anything but pre app start
            if (MvxFormsApplication.Current.MainPage == null)
            {
                return RequestType.PreAppStart;
            }

            return requestType;
        }

        public async Task<bool> TryShowAsync(MvxViewModelRequest request)
        {
            try
            {
                var requestType = GetRequestType(request);

                //Allow the navigation in case this is a root/modal request, otherwise ignore the request if busy
                if (_isBusy && (requestType != RequestType.Root))
                {
                    return false;
                }

                _isBusy = true;

                var page = MvxPresenterHelpers.CreatePage(request);
                (page as BaseView)?.OnInitialized();

                if (page == null)
                    return false;

                var viewModel = MvxPresenterHelpers.LoadViewModel(request);

                bool result;
                switch (requestType)
                {
                    case RequestType.MasterDetail:
                        result = await HandleMasterDetailRequest(page, viewModel);
                        break;
                    case RequestType.Root:
                        result = await HandleRootNavigationRequest(page, viewModel);
                        break;
                    case RequestType.Modal:
                        result = await HandleModalNavigationRequest(page, viewModel);
                        break;
                    case RequestType.PreAppStart:
                        result = await HandlePreAppStartRequest(page, viewModel);
                        break;
                    case RequestType.Clean:
                        result = await HandleCleanNavigationRequest(page, viewModel);
                        break;
                    case RequestType.None:
                        result = await HandleGeneralNavigationRequest(page, viewModel);
                        break;
                    default:
                        result = await HandleGeneralNavigationRequest(page, viewModel);
                        break;
                }

                if (result)
                {
                    OnPagePresented(page);
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Exception(nameof(TryShowAsync), ex);

                return false;
            }
            finally
            {
                IsShowingModalView = false;
                _isBusy = false;
            }
        }

        private async Task<bool> HandlePreAppStartRequest(Page page, IMvxViewModel viewModel)
        {
            page.BindingContext = viewModel;
            var nav = MvxFormsApplication.Current.MainPage as NavigationPage;
            if (nav == null)
            {
                nav = new NavigationPage(page);
                MvxFormsApplication.Current.MainPage = nav;
                CustomPlatformInitialization(nav);
                return true;
            }

            await nav.PushAsync(page, true);

            return true;
        }

        private async Task<bool> HandleMasterDetailRequest(Page page, IMvxViewModel viewModel)
        {
            var masterDetailPage = page as MasterDetailPage;
            if (masterDetailPage == null)
                return false;

            await CleanupMasterDetailPage();

            try
            {
                MvxFormsApplication.Current.MainPage = masterDetailPage;
            }
            catch (NullReferenceException) {
                // In IOS da problemi
            }

            RootMasterDetailPage = masterDetailPage;

            page.BindingContext = viewModel;
            return true;
        }

        private async Task CleanupMasterDetailPage()
        {
            if (!(RootMasterDetailPage?.Detail is NavigationPage))
                return;

            var navigation = RootMasterDetailPage.Detail.Navigation;
            foreach (var page in navigation.NavigationStack)
            {
                await CleanupViewModel(page);
            }

            await navigation.PopToRootAsync();
        }

        private async Task CleanupViewModel(Page page)
        {
            if (page == null)
                return;

            var vm = page.BindingContext as BaseViewModel;
            if (vm != null)
            {
                await vm.Cleanup();
            }

            //clean children also
            var tabbedPage = page as TabbedPage;
            if (tabbedPage != null)
            {
                foreach (var childPage in tabbedPage.Children)
                {
                    await CleanupViewModel(childPage);
                }
            }
        }

        private async Task<bool> HandleModalNavigationRequest(Page page, IMvxViewModel viewModel)
        {
            var view = page as BaseView;
            if (view != null)
            {
                view.ShowModalCloseButton = true;
            }

            page.BindingContext = viewModel;

            var navigationPage = new NavigationPage(page);

            IsShowingModalView = true;

            await MainPageNavigation.PushModalAsync(navigationPage);

            IsShowingModalView = false;

            return true;
        }

        public BaseViewModel TryGetTopmostViewModel()
        {
            var topmostView = TryGetTopmostView();
            return topmostView?.BindingContext as BaseViewModel;
        }

        /// <summary>
        ///     Looks in the entire navigation stack including modal pages.
        /// </summary>
        /// <returns></returns>
        public Page TryGetTopmostView()
        {
            var navigation = MainPageNavigation;

            //There is no modal page over the master detail page
            if (!navigation.ModalStack.Any() || navigation.ModalStack.Last() is MasterDetailPage)
            {
                return TryGetTopmostView(RootMasterDetailPage.Detail);
            }

            return TryGetTopmostView(navigation.ModalStack.Last());
        }

        /// <summary>
        ///     only looks in the current navigation stack. DOES NOT consider modal pages.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private Page TryGetTopmostView(Page root)
        {
            if (root == null)
                return null;

            if (root.Navigation.NavigationStack.Any())
            {
                var topmost = root.Navigation.NavigationStack.Last();
                return topmost;
            }

            return root;
        }

        private async Task<bool> HandleRootNavigationRequest(Page page, IMvxViewModel viewModel)
        {
            //Clean current stack before starting a new one
            if (RootMasterDetailPage.Detail != null)
            {
                await CleanupMasterDetailPage();
            }

            page.BindingContext = viewModel;

            RootMasterDetailPage.Detail = new NavigationPage(page);

            //runs smoother
            if (Device.RuntimePlatform == Device.Android)
            {
                await Task.Delay(200);
            }

            RootMasterDetailPage.IsPresented = false;

            return true;
        }

        private async Task<bool> HandleCleanNavigationRequest(Page page, IMvxViewModel viewModel)
        {
            var navigationPage = (NavigationPage)RootMasterDetailPage.Detail;

            //don't clean the first page in the stack just pop all others
            foreach (var childPage in navigationPage.Navigation.NavigationStack.Skip(1))
            {
                await CleanupViewModel(childPage);
            }

            await navigationPage.Navigation.PopToRootAsync(false);

            return await PresentPage(navigationPage, page, viewModel);
        }

        private async Task<bool> HandleGeneralNavigationRequest(Page page, IMvxViewModel viewModel)
        {
            NavigationPage mainPage = mainPage = RootMasterDetailPage != null
                ? (NavigationPage)RootMasterDetailPage.Detail
                : (NavigationPage)MvxFormsApplication.Current.MainPage;

            //check top of the modal navigation stack
            if (mainPage.Navigation.ModalStack.Any())
            {
                var last = mainPage.Navigation.ModalStack.Last() as NavigationPage;
                if (last != null)
                {
                    mainPage = last;
                }
            }

            return await PresentPage(mainPage, page, viewModel);
        }

        private async Task<bool> PresentPage(NavigationPage mainPage, Page page, IMvxViewModel viewModel)
        {
            if (Device.RuntimePlatform == Device.iOS && RootMasterDetailPage.IsPresented)
            {
                RootMasterDetailPage.IsPresented = false;
            }

            page.BindingContext = viewModel;
            await mainPage.PushAsync(page, true);

            if (Device.RuntimePlatform == Device.Android && RootMasterDetailPage.IsPresented)
            {
                RootMasterDetailPage.IsPresented = false;
            }

            return true;
        }

        #endregion
    }

    public class ViewModelPresentationAttribute : Attribute
    {
        public ViewModelPresentationAttribute(ViewModelPresentationType presentationType)
        {
            PresentationType = presentationType;
        }

        public ViewModelPresentationType PresentationType { get; }
    }

    public enum ViewModelPresentationType
    {
        AllowMultipleInstances,
        AllowSingleInstanceOnTop
    }
}