using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace BabyPOS_API.Tests
{
    public class ShopAndMenuUserFlowTests
    {
        private readonly HttpClient _client;
        public ShopAndMenuUserFlowTests()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5179/api/") };
        }

        [Fact(DisplayName = "สร้าง user vas, login, สร้างร้าน, เพิ่มเมนูอาหาร (สุ่มรูป)")]
        public async Task CreateUserShopAndMenuWithImage()
        {
            var username = "vas";
            var password = "1234";
            var email = $"{username}@mail.com";

            // Register (ignore error if already exists)
            try { await _client.PostAsJsonAsync("users/register", new { Username = username, Password = password, Email = email }); } catch { }

            // Login
            var loginRes = await _client.PostAsJsonAsync("users/login", new { Username = username, Password = password });
            loginRes.EnsureSuccessStatusCode();
            var loginObj = await loginRes.Content.ReadFromJsonAsync<LoginResult>();
            Assert.False(string.IsNullOrEmpty(loginObj?.token));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginObj.token);

            // Create shop (ไม่ต้องส่ง OwnerId)
            var shopName = $"ร้านของ {username}";
            var shopRes = await _client.PostAsJsonAsync("shops", new { Name = shopName });
            shopRes.EnsureSuccessStatusCode();
            var shopObj = await shopRes.Content.ReadFromJsonAsync<ShopResult>();

            // Prepare menu data
            var foodNames = new[] { "ข้าวผัด", "ชาเย็น", "ไอศกรีม" };
            var prices = new[] { 50, 25, 35 };
            var categories = new[] { 0, 2, 1 }; // MainDish, Drink, Dessert
            var imgFiles = Directory.GetFiles(@"D:\AI\workspace\BabyPOS\BabyPOS-Web\wwwroot\img\food").Where(f => f.EndsWith(".jpg") || f.EndsWith(".png")).ToArray();
            var rand = new Random();

            for (int i = 0; i < foodNames.Length; i++)
            {
                var img = imgFiles[rand.Next(imgFiles.Length)];
                var imgName = Path.GetFileName(img);
                var imagePath = $"/img/food/{imgName}";
                var menuRes = await _client.PostAsJsonAsync("menuitems", new {
                    Name = foodNames[i],
                    Price = prices[i],
                    ShopId = shopObj.Id,
                    Category = categories[i],
                    ImagePath = imagePath
                });
                menuRes.EnsureSuccessStatusCode();
                var menuItem = await menuRes.Content.ReadFromJsonAsync<MenuItemResult>();
                Assert.Equal(foodNames[i], menuItem.Name);
                Assert.Equal(shopObj.Id, menuItem.ShopId);
                Assert.Equal(categories[i], (int)menuItem.Category);
                Assert.Equal(imagePath, menuItem.ImagePath);
            }
        }

        public class LoginResult { public string token { get; set; } = string.Empty; }
        public class ShopResult { public int Id { get; set; } public string Name { get; set; } = string.Empty; public int OwnerId { get; set; } }
        public class MenuItemResult { public int Id { get; set; } public string Name { get; set; } = string.Empty; public int ShopId { get; set; } public int Category { get; set; } public string ImagePath { get; set; } = string.Empty; }
    }
}
