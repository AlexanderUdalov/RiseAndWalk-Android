using Newtonsoft.Json;
using RiseAndWalk_Android.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RiseAndWalk_Android.Controllers
{
    public class NetworkController
    {
        private const string AlarmServiceUrl = "https://localhost:44343/api/alarms";

        #region Singletone

        private static readonly Lazy<NetworkController> _instanceHolder =
            new Lazy<NetworkController>(() => new NetworkController());

        public static NetworkController Instance => _instanceHolder.Value;

        #endregion Singletone

        private HttpClient _client = new HttpClient();

        private async Task<List<Alarm>> GetAsync()
        {
            var response = await _client.GetAsync(AlarmServiceUrl);
            var stringContent = await response.Content.ReadAsStringAsync();

            return await Task.Run(() =>
                JsonConvert.DeserializeObject<List<Alarm>>(stringContent) ?? new List<Alarm>());
        }

        private async Task<bool> PostAsync(Alarm alarm)
        {
            var content = new StringContent(JsonConvert.SerializeObject(alarm), Encoding.UTF8, "application/json");

            var postResponse = await _client.PostAsync(AlarmServiceUrl, content);
            return postResponse.IsSuccessStatusCode;
        }

        public async Task DeleteAsync(Alarm alarm, HttpClient client)
            => await client.DeleteAsync(AlarmServiceUrl + "/" + alarm.Id);

        private async Task<string> RegisterAsync(UserModel user)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var postResponse = await _client.PostAsync("https://localhost:44343/api/account/register", stringContent);

            var content = await postResponse.Content.ReadAsStringAsync();

            if (!postResponse.IsSuccessStatusCode) return null;

            var token = content;
            if (string.IsNullOrEmpty(token)) return null;

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return token;
        }

        private async Task<string> LoginAsync(UserModel user)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var postResponse = await _client.PostAsync("https://localhost:44343/api/account/login", stringContent);

            var content = await postResponse.Content.ReadAsStringAsync();

            if (!postResponse.IsSuccessStatusCode) return null;

            var token = content;
            if (string.IsNullOrEmpty(token)) return null;

            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return token;
        }

        private bool Logout(HttpClient client)
            => client.DefaultRequestHeaders.Remove("Authorization");
    }
}