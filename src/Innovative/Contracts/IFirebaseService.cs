using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSY.Innovative.Models;

namespace PSY.Innovative.Contracts
{
    public interface IFirebaseService
    {
        string RefreshedToken { get; set;}
        
    }
}
