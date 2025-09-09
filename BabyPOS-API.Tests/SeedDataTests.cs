using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;

namespace BabyPOS_API.Tests
{
    public class SeedDataTests
    {
        private readonly HttpClient _client;
        public SeedDataTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:5179/api/") };
        }

        [Fact(DisplayName = "Seed users, shops, and menu items via API")] 
        public async Task SeedUsersShopsMenuItems()
        {
            // 1. Register 5 users
            var users = new List<(string Username, string Password, string Email)>
            {
                ("alice", "123456", "alice@example.com"),
                ("bob", "123456", "bob@example.com"),
                ("carol", "123456", "carol@example.com"),
                ("dave", "123456", "dave@example.com"),
                ("eve", "123456", "eve@example.com")
            };
            var userIds = new List<int>();
            foreach (var u in users)
            {
                var regRes = await _client.PostAsJsonAsync("users/register", new { Username = u.Username, Password = u.Password, Email = u.Email });
                regRes.EnsureSuccessStatusCode();
                var regObj = await regRes.Content.ReadFromJsonAsync<UserResult>();
                userIds.Add(regObj.Id);
            }

            // 2. Create 2 shops (owner: alice, bob)
            var shop1 = new { Name = "ร้าน Alice", OwnerId = userIds[0] };
            var shop2 = new { Name = "ร้าน Bob", OwnerId = userIds[1] };
            var shopRes1 = await _client.PostAsJsonAsync("shops", shop1);
            var shopRes2 = await _client.PostAsJsonAsync("shops", shop2);
            shopRes1.EnsureSuccessStatusCode();
            shopRes2.EnsureSuccessStatusCode();
            var shopObj1 = await shopRes1.Content.ReadFromJsonAsync<ShopResult>();
            var shopObj2 = await shopRes2.Content.ReadFromJsonAsync<ShopResult>();

            // 3. Add menu items to each shop
            var menuItems = new[]
            {
                new { Name = "ข้าวผัด", Price = 50, ShopId = shopObj1.Id },
                new { Name = "ต้มยำกุ้ง", Price = 80, ShopId = shopObj1.Id },
                new { Name = "ผัดไทย", Price = 60, ShopId = shopObj1.Id },
                new { Name = "ชาเย็น", Price = 25, ShopId = shopObj2.Id },
                new { Name = "กาแฟ", Price = 30, ShopId = shopObj2.Id },
                new { Name = "ขนมปังปิ้ง", Price = 20, ShopId = shopObj2.Id }
            };
            foreach (var m in menuItems)
            {
                var menuRes = await _client.PostAsJsonAsync("menuitems", m);
                menuRes.EnsureSuccessStatusCode();
            }
        }

        public class UserResult { public int Id { get; set; } }
        public class ShopResult { public int Id { get; set; } }
    }
}
