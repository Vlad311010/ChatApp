using ChatApp.Client.Models;
using System.Net.Http;
using System;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ChatApp.Client.Services
{
    public class ApiAccess
    {
        private readonly HttpClient httpClient;

        public ApiAccess(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Message>> GetMessagesAsync(string ChatName)
        {
            List<Message>? messages = await httpClient.GetFromJsonAsync<List<Message>>(string.Format("api/chat/{0}/messages", ChatName));
            return messages == null ? new List<Message>() : messages;
        }

        public async Task<UserData?> LoginAsync(UserData loginRequest)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                UserData? userData = JsonSerializer.Deserialize<UserData>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true } );
                return userData;
            }

            return null;
        }

        public async Task LogoutAsync()
        {
            HttpResponseMessage response = await httpClient.PostAsync("api/logout", null);
        }

        public async Task<UserData?> AuthenticationRefreshAsync()
        {
            HttpResponseMessage response = await httpClient.PostAsync("api/AutRefresh", null);
            response.EnsureSuccessStatusCode();
            
            string responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
                return null;

            UserData? userData = JsonSerializer.Deserialize<UserData>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (userData == null || userData.Login == null)
                return null;
            else
                return userData;
        }
    }
}
