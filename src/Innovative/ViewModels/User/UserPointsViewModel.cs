using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PSY.Innovative.Resources;
using PSY.Innovative.Contracts;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace PSY.Innovative.ViewModels.User
{
    class UserPointsViewModel : ConnectionBaseViewModel
    {
        public ObservableCollection<Models.Point> Points => _dataManagerService.Points;

        public ICommand LoadPointsCommand { get; set; }

        public UserPointsViewModel(IDataManagerService dataManagerService, IRestService restService) : base(dataManagerService, restService)
        {
            Title = AppResources.UserPoints;

            LoadPointsCommand = new Command(async () => await ExecuteLoadPointsCommand());
        }

        async Task ExecuteLoadPointsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await _restService.GetPointsAsync(true);
            }
            catch (Exception)
            {
                
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
