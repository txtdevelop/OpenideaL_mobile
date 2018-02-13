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
    public class Vote : MvxViewModel, ILinkable
    {
        private int _id;
        [JsonProperty("vote_id")]
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

        private Idea _idea;
         [JsonProperty("idea")]
        public Idea Idea
        {
            get => _idea;
            set
            {
                _idea = value;
                RaisePropertyChanged(() => Idea);
            }
        }

        private VoteType _voteType;
        public VoteType VoteType
        {
            get => VoteType.Plus;// _voteType;
            set
            {
                _voteType = value;
                RaisePropertyChanged(() => VoteType);
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
            return $"http://vm-psymbiosys/innovanext/myuserpoints/transaction/{_id}/{linkType}";
        }
    }

    

    public enum VoteType
    {
        Minus,
        Plus
    }
}
