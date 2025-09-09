using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace BabyPOS_API.Tests
{
    public class ShopControllerUserFlowTests
    {
        private readonly HttpClient _client;
        public ShopControllerUserFlowTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:5179/api/") };
        }

        [Fact(DisplayName = "Register user, login, and create shop")] 
        public async Task RegisterLoginAndCreateShop()
        {
            var username = $"shopuser_{System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var password = "Test1234";
            var email = $"{username}@mail.com";

            // Register
            var regRes = await _client.PostAsJsonAsync("users/register", new { Username = username, Password = password, Email = email });
            regRes.EnsureSuccessStatusCode();
            var regObj = await regRes.Content.ReadFromJsonAsync<UserResult>();

            // Login
            var loginRes = await _client.PostAsJsonAsync("users/login", new { Username = username, Password = password });
            loginRes.EnsureSuccessStatusCode();
            var loginObj = await loginRes.Content.ReadFromJsonAsync<LoginResult>();
            Assert.False(string.IsNullOrEmpty(loginObj?.token));

            // Create shop
            var shopName = $"ร้านของ {username}";
            var shopRes = await _client.PostAsJsonAsync("shops", new { Name = shopName, OwnerId = regObj.Id });
            shopRes.EnsureSuccessStatusCode();
            var shopObj = await shopRes.Content.ReadFromJsonAsync<ShopResult>();
            Assert.Equal(shopName, shopObj.Name);
            Assert.Equal(regObj.Id, shopObj.OwnerId);
        }

        public class UserResult { public int Id { get; set; } }
        public class LoginResult { public string token { get; set; } = string.Empty; }
        public class ShopResult { public int Id { get; set; } public string Name { get; set; } = string.Empty; public int OwnerId { get; set; } }
    }
}
