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
    public class DataManagerService : IDataManagerService
    {
        public User User { get; set; }

        public ObservableCollection<Idea> Ideas { get; set; }

        public ObservableCollection<Point> Points { get; set; }

        public ObservableCollection<Vote> Votes { get; set; }

        public DataManagerService()
        {
            Ideas = new ObservableCollection<Idea>();
            Points = new ObservableCollection<Point>();
            Votes = new ObservableCollection<Vote>();
        }
    }
}
