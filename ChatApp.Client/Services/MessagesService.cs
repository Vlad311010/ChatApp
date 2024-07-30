using ChatApp.Client.Models;
using System.Net.Http.Json;

namespace ChatApp.Client.Services
{
    public class MessagesService
    {
        private readonly HttpClient httpClient;

        public MessagesService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Message>>("api/chat/Test01/messages");
        }
    }
}
