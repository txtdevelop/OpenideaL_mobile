using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using PSY.Innovative.Contracts;
using PSY.Innovative.Resources;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace PSY.Innovative.ViewModels
{
    public class HomeViewModel : ConnectionBaseViewModel
    {
        public ObservableCollection<Models.Idea> Ideas => _dataManagerService.Ideas;

        public ICommand LoadIdeasCommand { get; set; }

        public HomeViewModel(IDataManagerService dataManagerService, IRestService restService) : base(dataManagerService, restService)
        {
            Title = AppResources.Home;

            LoadIdeasCommand = new Command(async () => await ExecuteLoadIdeasCommand());
        }

        async Task ExecuteLoadIdeasCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await _restService.GetIdeasAsync(true);
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
