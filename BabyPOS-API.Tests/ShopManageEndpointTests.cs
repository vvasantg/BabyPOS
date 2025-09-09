using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;

namespace BabyPOS_API.Tests
{
    public class ShopManageEndpointTests
    {
        private readonly HttpClient _client;
        public ShopManageEndpointTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:5179/api/") };
        }

        [Fact(DisplayName = "GET /api/shops/manage returns only shops of the owner (JWT)")]
        public async Task GetShopsManage_ReturnsOwnerShopsOnly()
        {
            // Register and login a new user
            var username = $"owner_{System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var password = "Test1234";
            var email = $"{username}@mail.com";
            await _client.PostAsJsonAsync("users/register", new { Username = username, Password = password, Email = email });
            var loginRes = await _client.PostAsJsonAsync("users/login", new { Username = username, Password = password });
            loginRes.EnsureSuccessStatusCode();
            var loginObj = await loginRes.Content.ReadFromJsonAsync<LoginResult>();
            Assert.False(string.IsNullOrEmpty(loginObj?.token));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginObj.token);

            // Create 2 shops for this user
            var shopNames = new[] { $"ร้าน {username} 1", $"ร้าน {username} 2" };
            foreach (var shopName in shopNames)
            {
                var shopRes = await _client.PostAsJsonAsync("shops", new { Name = shopName });
                shopRes.EnsureSuccessStatusCode();
            }

            // Call /api/shops/manage
            var manageRes = await _client.GetAsync("shops/manage");
            manageRes.EnsureSuccessStatusCode();
            var shops = await manageRes.Content.ReadFromJsonAsync<List<ShopResult>>();
            Assert.NotNull(shops);
            Assert.Equal(2, shops.Count);
            Assert.All(shops, s => Assert.Contains(username, s.Name));
        }

        public class LoginResult { public string token { get; set; } = string.Empty; }
        public class ShopResult { public int Id { get; set; } public string Name { get; set; } = string.Empty; public int OwnerId { get; set; } }
    }
}
