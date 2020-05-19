using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Models;
using Jobtech.OpenPlatforms.MockGigPlatform.Api.Models.GigCentral;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace Jobtech.OpenPlatforms.MockGigPlatform.Api.Handlers
{
    public class MockPlatformHttpClient : IMockPlatformHttpClient
    {
        private readonly HttpClient _client;

        public MockPlatformHttpClient(HttpClient client)
        {
            _client = client;
        }

        private IEnumerable<UserModel> _users;

        public async Task AddUserAsync(User user)
        {
            _users = _users ?? await GetUsersAsync();

            if (_users.Any(u => u.Email == user.UserEmail))
            {
                return;
            }

            // Requires GigCentral to be running on port 5001 https
            var urlToAdd = "http://localhost:5003/api/user/add";
            var _clientUri = new Uri(urlToAdd);

            var content = new StringContent(JsonConvert.SerializeObject(user.ToGigCentralUserModel()), Encoding.UTF8, "application/json");
            try
            {
                var result = await _client.PostAsync(_clientUri, content);

                result.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                // Log this?
            }
        }

        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            var urlToGet = "http://localhost:5003/api/user/";
            var _clientUri = new Uri(urlToGet);

            //_client.BaseAddress = new Uri(urlToGet);
            try
            {
                var response = await _client.GetAsync(_clientUri);
                response.EnsureSuccessStatusCode();
                var stringResult = await response.Content.ReadAsStringAsync();

                if (stringResult == "[]") return new List<UserModel>();

                return JsonConvert.DeserializeObject<List<UserModel>>(stringResult);
            }
            catch (Exception)
            {
                return new List<UserModel>();
            }
        }
    }
}