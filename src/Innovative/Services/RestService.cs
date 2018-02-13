using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PSY.Innovative.Contracts;
using PSY.Innovative.Models;
using System.Net.Http;
using System.Threading;
using MvvmCross.Platform.Core;
using Newtonsoft.Json;
using PSY.Innovative.Helpers;
using System.Collections.ObjectModel;

namespace PSY.Innovative.Services
{
    public class RestService : RestApi, IRestService
    {

        private readonly HttpClient _httpClient;
        private readonly IDataManagerService _dataManagerService;
        private readonly IFirebaseService _firebaseService;

        public int IdUser { get; set; }

        public RestService(IDataManagerService dataManagerService, IFirebaseService firebaseService)
        {
            _dataManagerService = dataManagerService;
            _firebaseService = firebaseService;

            _httpClient = new HttpClient();
        }

        public async Task<User> LoginAsync(string username, string password, bool updateDataManager)
        {
            User returnValue = null;

            var cancellationTokenSource = CreateCancellationTokenSource();
            try
            {
                IdUser = 0;
                var json = JsonConvert.SerializeObject(new Dictionary<string, string>()
                {
                    {"username", username},
                    {"password", password}
                });
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync(new Uri(LoginUrl), httpContent, cancellationTokenSource.Token);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    returnValue = JsonConvert.DeserializeObject<User>(await responseMessage.Content.ReadAsStringAsync());

                    if (updateDataManager)
                    {
                        IdUser = returnValue.Id;
                        _dataManagerService.User = returnValue;
                    }

                    await UpdateRefreshedTokenAsync();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            finally
            {
                cancellationTokenSource?.DisposeIfDisposable();
            }

            return returnValue;
        }

        public async Task<bool> LogoutAsync(bool updateDataManager)
        {
            bool returnValue = false;

            if (updateDataManager)
            {
                this.IdUser = 0;
                _dataManagerService.User = null;
                _dataManagerService.Ideas.Clear();
                _dataManagerService.Points.Clear();
                _dataManagerService.Votes.Clear();
            }

            await UpdateRefreshedTokenAsync(true);

            return returnValue;
        }

        public async Task<User> GetUserAsync(bool updateDataManager)
        {
            User returnValue = null;

            var cancellationTokenSource = CreateCancellationTokenSource();
            try
            {
                var responseMessage = await _httpClient.GetAsync(new Uri($"{GetUserUrl}?uid={IdUser}"), cancellationTokenSource.Token);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    returnValue = JsonConvert.DeserializeObject<User>(await responseMessage.Content.ReadAsStringAsync());

                    if (updateDataManager)
                    {
                        _dataManagerService.User = returnValue;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            finally
            {
                cancellationTokenSource?.DisposeIfDisposable();
            }

            return returnValue;
        }

        public async Task<IEnumerable<Idea>> GetIdeasAsync(bool updateDataManager)
        {
            IList<Idea> returnValue = null;

            var cancellationTokenSource = CreateCancellationTokenSource();
            try
            {
                var responseMessage = await _httpClient.GetAsync(new Uri($"{GetIdeasUrl}?uid={IdUser}"), cancellationTokenSource.Token);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    returnValue = JsonConvert.DeserializeObject<List<Idea>>(await responseMessage.Content.ReadAsStringAsync());

                    if (updateDataManager)
                    {
                        _dataManagerService.Ideas.Clear();
                        foreach (var t in returnValue)
                        {
                            _dataManagerService.Ideas.Add(t);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            finally
            {
                cancellationTokenSource?.DisposeIfDisposable();
            }

            return returnValue;
        }

        public async Task<IEnumerable<Point>> GetPointsAsync(bool updateDataManager)
        {
            List<Point> returnValue = null;

            var cancellationTokenSource = CreateCancellationTokenSource();
            try
            {
                var responseMessage = await _httpClient.GetAsync(new Uri($"{GetPointsUrl}?uid={IdUser}"), cancellationTokenSource.Token);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    returnValue = JsonConvert.DeserializeObject<List<Point>>(await responseMessage.Content.ReadAsStringAsync());

                    if (updateDataManager)
                    {
                        _dataManagerService.Points.Clear();
                        foreach (var t in returnValue)
                        {
                            _dataManagerService.Points.Add(t);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            finally
            {
                cancellationTokenSource?.DisposeIfDisposable();
            }

            return returnValue;
        }

        public async Task<IEnumerable<Vote>> GetVotesAsync(bool updateDataManager)
        {
            IList<Vote> returnValue = null;

            var cancellationTokenSource = CreateCancellationTokenSource();
            try
            {
                var responseMessage = await _httpClient.GetAsync(new Uri($"{GetVotesUrl}?uid={IdUser}"), cancellationTokenSource.Token);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    returnValue = JsonConvert.DeserializeObject<List<Vote>>(await responseMessage.Content.ReadAsStringAsync());

                    if (updateDataManager)
                    {
                        _dataManagerService.Votes.Clear();
                        foreach (var t in returnValue)
                        {
                            _dataManagerService.Votes.Add(t);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            finally
            {
                cancellationTokenSource?.DisposeIfDisposable();
            }

            return returnValue;
        }

        public async Task<bool> UpdateRefreshedTokenAsync(bool emptyRefreshToken = false)
        {
            bool returnValue = false;

            if (IdUser == 0 || _firebaseService.RefreshedToken == null || _firebaseService.Equals(""))
            {
                return returnValue;
            }

            string refreshedToken = _firebaseService.RefreshedToken;
            if (emptyRefreshToken)
            {
                refreshedToken = null;
            }

            var cancellationTokenSource = CreateCancellationTokenSource();
            try
            {
                var json = JsonConvert.SerializeObject(new Dictionary<string, string>()
                {
                    {"uid", IdUser.ToString()},
                    {"refreshedToken", refreshedToken}
                });
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync(new Uri(UpdateRefreshedTokenUrl), httpContent, cancellationTokenSource.Token);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    //returnValue = JsonConvert.DeserializeObject<User>(await responseMessage.Content.ReadAsStringAsync());

                    returnValue = true;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
            finally
            {
                cancellationTokenSource?.DisposeIfDisposable();
            }

            return returnValue;
        }

        private static CancellationTokenSource CreateCancellationTokenSource()
        {
            return new CancellationTokenSource(/*new TimeSpan(0, 0, 30)*/);
        }
    }

    public class RestApi
    {
        private const string BaseUrl = "http://vm-psymbiosys/innovanext/sites/all/modules/mobile_api/api/";

        protected const string LoginUrl = BaseUrl + "login.php";
        protected const string LogoutUrl = BaseUrl + "logout.php";
        protected const string GetUserUrl = BaseUrl + "getUser.php";
        protected const string GetPointsUrl = BaseUrl + "getPoints.php";
        protected const string GetVotesUrl = BaseUrl + "getVotes.php";
        protected const string GetIdeasUrl = BaseUrl + "getIdeas.php";

        protected const string UpdateRefreshedTokenUrl = BaseUrl + "updateRefreshedToken.php";
    }
}
