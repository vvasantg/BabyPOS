using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using BabyPOS_Web.Models;
using BabyPOS_Web.ViewModels;

namespace BabyPOS_Web.Tests
{
    public class LoginViewModelTests
    {
        private readonly HttpClient _http;
        public LoginViewModelTests()
        {
            // ??? base address ??? config web
            _http = new HttpClient { BaseAddress = new System.Uri("http://localhost:5179/api/") };
        }

        [Fact]
        public async Task Login_Success()
        {
            // ?????? user ????????????? (??????????????????????)
            var username = "testuserweb";
            var password = "Test1234";
            var email = "testuserweb@mail.com";

            // ????? user (ignore error ?????????????)
            await _http.PostAsJsonAsync("users/register", new { Username = username, Password = password, Email = email });

            // ????? login
            var vm = new LoginViewModel(_http) { Username = username, Password = password };
            var result = await vm.LoginAsync();
            Assert.True(result);
            Assert.False(string.IsNullOrEmpty(vm.Token));
            Assert.NotNull(vm.CurrentUser);
            Assert.Equal(username, vm.CurrentUser.Username);
        }
    }
}
