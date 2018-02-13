using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSY.Innovative.Contracts;
using PSY.Innovative.Resources;

namespace PSY.Innovative.ViewModels.User
{
    class UserInfoViewModel : ConnectionBaseViewModel
    {
        public Models.User User => _dataManagerService.User;

        public UserInfoViewModel(IDataManagerService dataManagerService, IRestService restService) : base(dataManagerService, restService)
        {
            Title = AppResources.UserInfo;
        }
    }
}
