using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSY.Innovative.Models;
using System.Collections.ObjectModel;

namespace PSY.Innovative.Contracts
{
    public interface IDataManagerService
    {
        User User { get; set; }

        ObservableCollection<Idea> Ideas { get; set; }

        ObservableCollection<Point> Points { get; set; }

        ObservableCollection<Vote> Votes { get; set; }
    }
}
