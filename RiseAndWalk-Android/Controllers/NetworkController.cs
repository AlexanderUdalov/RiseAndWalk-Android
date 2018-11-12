using Newtonsoft.Json;
using RiseAndWalk_Android.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Preferences;

//TODO: выделить account controller с контекстом, данный класс только для взаимодействия с сетью
namespace RiseAndWalk_Android.Controllers
{
    public class NetworkController
    {
        private const string AlarmServiceUrl = "http://192.168.43.155:5000/api/alarms";
        private const string AccountServiceUrl = "http://192.168.43.155:5000/api/account/";

        #region Singletone

        private static readonly Lazy<NetworkController> _instanceHolder =
            new Lazy<NetworkController>(() => new NetworkController());

        public static NetworkController Instance => _instanceHolder.Value;

        #endregion Singletone

        private readonly HttpClient _client = new HttpClient();

        public void SetToken(string token)
        {
            Console.Write(_client.DefaultRequestHeaders?.Authorization?.Scheme);
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

        }

        public async Task<List<Alarm>> GetAsync()
        {
            var response = await _client.GetAsync(AlarmServiceUrl).ConfigureAwait(false);
            var stringContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return await Task.Run(() =>
                JsonConvert.DeserializeObject<List<Alarm>>(stringContent) ?? new List<Alarm>());
        }

        public async Task<bool> PostAsync(Alarm alarm)
        {
            var content = new StringContent(JsonConvert.SerializeObject(alarm), Encoding.UTF8, "application/json");

            var postResponse = await _client.PostAsync(AlarmServiceUrl, content).ConfigureAwait(false);
            return postResponse.IsSuccessStatusCode;
        }

        public async Task DeleteAsync(Alarm alarm, HttpClient client)
            => await client.DeleteAsync(AlarmServiceUrl + "/" + alarm.Id).ConfigureAwait(false);

        public void SaveToken(Context context, string token)
        {
            var pref = PreferenceManager.GetDefaultSharedPreferences(context);
            var prefEditor = pref.Edit();
            prefEditor.PutString("userToken", token);
            prefEditor.Commit();
        }

        public async Task<string> RegisterAsync(UserModel user)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var postResponse = await _client.PostAsync(AccountServiceUrl + "register", stringContent).ConfigureAwait(false);

            var content = await postResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!postResponse.IsSuccessStatusCode) return null;

            var token = content;
            return string.IsNullOrEmpty(token) ? null : token;
        }

        public async Task<string> LoginAsync(UserModel user)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var postResponse = await _client.PostAsync(AccountServiceUrl + "login", stringContent).ConfigureAwait(false);

            var content = await postResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!postResponse.IsSuccessStatusCode) return null;

            var token = content;
            return string.IsNullOrEmpty(token) ? null : token;
        }

        public bool Logout(HttpClient client)
            => client.DefaultRequestHeaders.Remove("Authorization");
    }
}