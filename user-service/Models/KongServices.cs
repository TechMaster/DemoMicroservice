using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace user_service.Models
{
    public class KongService
    {
        public HttpClient Client { get; set; }
        public const string ApiUri = "http://localhost:8001/";

        public KongService()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ApiUri);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // API document: https://getkong.org/docs/0.12.x/admin-api/#create-consumer
        public async Task<HttpResponseMessage> CreateConsumer(string customId)
        {
            HttpResponseMessage createConsumer = await Client.PostAsJsonAsync(
                "consumers", new { username = customId, custom_id = customId });

            HttpResponseMessage createCredential = await Client.PostAsJsonAsync(
                $"consumers/{customId}/jwt", new { key = "user-service", secret = "jwts-are-awesome" });

            return createCredential;
        }

        // API document: https://getkong.org/docs/0.12.x/admin-api/#delete-consumer
        public async Task<HttpResponseMessage> DeleteConsumer(string customId)
        {
            HttpResponseMessage response = await Client.DeleteAsync("consumers/" + customId);
            return response;
        }
    }
}