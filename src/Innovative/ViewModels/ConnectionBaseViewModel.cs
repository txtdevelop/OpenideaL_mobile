using System.Threading.Tasks;
using PSY.Innovative.Contracts;

namespace PSY.Innovative.ViewModels
{
    /// <summary>
    ///     Should be used by all view models/views that are using a connected device or need tho show the connection
    ///     indicator.
    /// </summary>
    public abstract class ConnectionBaseViewModel : BaseViewModel
    {
        protected readonly IDataManagerService _dataManagerService;
        protected readonly IRestService _restService;

        #region C'tor

        protected ConnectionBaseViewModel(IDataManagerService dataManagerService, IRestService restService) : base()
        {
            _restService = restService;
            _dataManagerService = dataManagerService;
        }


        #endregion

        #region Properties


        #endregion

        #region LifecycleEvents

        public override void Start()
        {
            base.Start();

            //Register here for the events from the services
        }

        public override void Loaded()
        {
            base.Loaded();
            UpdateStatus();
        }

        public override void Resumed()
        {
            base.Resumed();
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            
        }


        public override Task Cleanup()
        {
            //UnRegister here for the events from the services
            return base.Cleanup();
        }

        #endregion
    }

    public enum CurrentDeviceState
    {
        Offline,
        Online
    }
}