using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace PSY.Innovative.Models
{
    public class Idea : MvxViewModel, ILinkable
    {
        private int _id;
        [JsonProperty("nid")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                LinkEdit = GetLink(value, LinkType.edit);
                LinkView = GetLink(value, LinkType.view);
                RaisePropertyChanged(() => Id);
            }
        }

        private User _user;
        [JsonProperty("user")]
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                RaisePropertyChanged(() => User);
            }
        }

        private string _title;
        [JsonProperty("title")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private string _linkView;
        public string LinkView
        {
            get => _linkView;
            set
            {
                _linkView = value;
                RaisePropertyChanged(() => LinkView);
            }
        }

        private string _linkEdit;
        public string LinkEdit
        {
            get => _linkEdit;
            set
            {
                _linkEdit = value;
                RaisePropertyChanged(() => LinkEdit);
            }
        }

        public string GetLink(int id, LinkType linkType)
        {
            return "http://vm-psymbiosys/innovanext/myuserpoints/transaction/${_id}/${linkType}";
        }
    }
}
