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
    public class Point : MvxViewModel, ILinkable
    {
        private int _id;
        [JsonProperty("txn_id")]
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
        public Idea Idea
        {
            get => _idea;
            set
            {
                _idea = value;
                RaisePropertyChanged(() => Idea);
            }
        }

        private int _pointValue;
        [JsonProperty("points")]
        public int PointValue
        {
            get => _pointValue;
            set
            {
                _pointValue = value;
                RaisePropertyChanged(() => PointValue);
            }
        }

        private DateTime _date;
        [JsonProperty("created_at")]
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        private Reason _reason;
        [JsonProperty("operation")]
        public Reason Reason
        {
            get => _reason;
            set
            {
                _reason = value;
                RaisePropertyChanged(() => Reason);
            }
        }

        private Status _status;
        public Status Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
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

    public enum Reason
    {
        Insert
    }

    public enum Status
    {
        Approved,
        Declined,
        Pending
    }
}
