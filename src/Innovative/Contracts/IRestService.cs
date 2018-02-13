using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSY.Innovative.Models;

namespace PSY.Innovative.Contracts
{
    public interface IRestService
    {
        int IdUser {get; set;}

        Task<User> LoginAsync(string username, string password, bool updateDataManager);

        Task<bool> LogoutAsync(bool updateDataManager);

        Task<User> GetUserAsync(bool updateDataManager);

        Task<IEnumerable<Idea>> GetIdeasAsync(bool updateDataManager);

        Task<IEnumerable<Point>> GetPointsAsync(bool updateDataManager);

        Task<IEnumerable<Vote>> GetVotesAsync(bool updateDataManager);

        Task<bool> UpdateRefreshedTokenAsync(bool emptyRefreshToken = false);
    }
}
