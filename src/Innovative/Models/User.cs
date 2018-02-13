using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace PSY.Innovative.Models
{
    public class User : MvxViewModel
    {
        private int _id;
        [JsonProperty("uid")]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string _mail;
        [JsonProperty("mail")]
        public string Mail
        {
            get => _mail;
            set
            {
                _mail = value;
                RaisePropertyChanged(() => Mail);
            }
        }

        private string _firstName;
        [JsonProperty("first_name")]
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private string _lastName;
        [JsonProperty("last_name")]
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                RaisePropertyChanged(() => Address);
            }
        }

        private int _numberOfIdeas;
        [JsonProperty("ideas_number")]
        public int NumberOfIdeas
        {
            get => _numberOfIdeas;
            set
            {
                _numberOfIdeas = value;
                RaisePropertyChanged(() => NumberOfIdeas);
            }
        }

        private int _numberOfPoints;
        [JsonProperty("points_number")]
        public int NumberOfPoints
        {
            get => _numberOfPoints;
            set
            {
                _numberOfPoints = value;
                RaisePropertyChanged(() => NumberOfPoints);
            }
        }

        private string _srcProfileImage;
        [JsonProperty("uri")]
        public string SrcProfileImage
        {
            get => _srcProfileImage;
            set
            {
                _srcProfileImage = value;
                ProfileImage = GetImageSource(value);
                RaisePropertyChanged(() => SrcProfileImage);
            }
        }

        private ImageSource _profileImage;
        public ImageSource ProfileImage
        {
            get => _profileImage;
            set
            {
                _profileImage = value;
                RaisePropertyChanged(() => ProfileImage);
            }
        }

        private static ImageSource GetImageSource(string src)
        {
            if (src != null && Uri.TryCreate(src, UriKind.Absolute, out Uri uri))
            {
                return ImageSource.FromUri(uri);
            }
            else
            {
                return "userCircle";
            }
        }
    }
}
