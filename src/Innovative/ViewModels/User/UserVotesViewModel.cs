using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PSY.Innovative.Contracts;
using PSY.Innovative.Resources;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace PSY.Innovative.ViewModels.User
{
    class UserVotesViewModel : ConnectionBaseViewModel
    {
        public ObservableCollection<Models.Vote> Votes => _dataManagerService.Votes;

        public ICommand LoadVotesCommand { get; set; }

        public UserVotesViewModel(IDataManagerService dataManagerService, IRestService restService) : base(dataManagerService, restService)
        {
            Title = AppResources.UserVotes;

            LoadVotesCommand = new Command(async () => await ExecuteLoadVotesCommand());
        }

        async Task ExecuteLoadVotesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await _restService.GetVotesAsync(true);
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
