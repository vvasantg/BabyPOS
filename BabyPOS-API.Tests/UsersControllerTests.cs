using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace BabyPOS_API.Tests
{
    public class UsersControllerTests
    {
        private readonly HttpClient _client;

        public UsersControllerTests()
        {
            // ???? base address ????????? API ????
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:5179/api/") };
        }

        [Fact]
        public async Task Register_And_Login_User_Success()
        {
            var username = $"testuser_{System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var password = "Test1234";
            var email = $"{username}@mail.com";

            // Register
            var registerResponse = await _client.PostAsJsonAsync("users/register", new
            {
                Username = username,
                Password = password,
                Email = email
            });
            registerResponse.EnsureSuccessStatusCode();

            // Login
            var loginResponse = await _client.PostAsJsonAsync("users/login", new
            {
                Username = username,
                Password = password
            });
            loginResponse.EnsureSuccessStatusCode();

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();
            Assert.False(string.IsNullOrEmpty(loginResult?.token));
            Assert.Equal(username, loginResult?.user?.Username);
        }

        public class LoginResult
        {
            public string token { get; set; } = string.Empty;
            public UserResult user { get; set; } = new UserResult();
        }
        public class UserResult
        {
            public int Id { get; set; }
            public string Username { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }
    }
}
