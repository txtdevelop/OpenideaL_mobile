using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSY.Innovative.Contracts;
using PSY.Innovative.Models;
using System.Collections.ObjectModel;

namespace PSY.Innovative.Services
{
    public class FirebaseService : IFirebaseService
    {
        public string RefreshedToken { get; set; }

    }
}
