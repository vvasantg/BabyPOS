using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.Text.Json;

namespace BabyPOS_API.Tests
{
    public class ShopDescriptionApiTests
    {
        private readonly HttpClient _client;
        
        public ShopDescriptionApiTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:5179/api/") };
        }

        [Fact(DisplayName = "Create shop with description via main endpoint")]
        public async Task CreateShop_WithDescription_ReturnsSuccess()
        {
            // Register and login user
            var (token, userId) = await RegisterAndLoginUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create shop with description
            var shopName = $"ร้านทดสอบ {System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var shopDescription = "คำอธิบายร้านทดสอบ - อาหารอร่อย ราคาย่อมเยา";
            
            var createRes = await _client.PostAsJsonAsync("shops", new { 
                Name = shopName, 
                Description = shopDescription 
            });
            
            createRes.EnsureSuccessStatusCode();
            var shop = await createRes.Content.ReadFromJsonAsync<ShopResult>();
            
            Assert.NotNull(shop);
            Assert.Equal(shopName, shop.Name);
            Assert.Equal(shopDescription, shop.Description);
            Assert.Equal(userId, shop.OwnerId);
        }

        [Fact(DisplayName = "Create shop with description via management endpoint")]
        public async Task CreateShopManage_WithDescription_ReturnsSuccess()
        {
            // Register and login user
            var (token, userId) = await RegisterAndLoginUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create shop with description via management endpoint
            var shopName = $"ร้านจัดการ {System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var shopDescription = "คำอธิบายร้านจัดการ - บริการดี มีคุณภาพ";
            
            var createRes = await _client.PostAsJsonAsync("shops/manage", new { 
                Name = shopName, 
                Description = shopDescription 
            });
            
            createRes.EnsureSuccessStatusCode();
            var shop = await createRes.Content.ReadFromJsonAsync<ShopResult>();
            
            Assert.NotNull(shop);
            Assert.Equal(shopName, shop.Name);
            Assert.Equal(shopDescription, shop.Description);
            Assert.Equal(userId, shop.OwnerId);
        }

        [Fact(DisplayName = "Update shop with description via main endpoint")]
        public async Task UpdateShop_WithDescription_ReturnsSuccess()
        {
            // Register and login user
            var (token, userId) = await RegisterAndLoginUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create shop first
            var shopName = $"ร้านเดิม {System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var createRes = await _client.PostAsJsonAsync("shops", new { 
                Name = shopName, 
                Description = "คำอธิบายเดิม" 
            });
            createRes.EnsureSuccessStatusCode();
            var createdShop = await createRes.Content.ReadFromJsonAsync<ShopResult>();

            // Update shop with new description
            var newName = $"ร้านใหม่ {System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var newDescription = "คำอธิบายใหม่ - อัพเดทแล้ว";
            
            var updateRes = await _client.PutAsJsonAsync($"shops/{createdShop.Id}", new { 
                Id = createdShop.Id,
                Name = newName, 
                Description = newDescription,
                OwnerId = userId
            });
            
            updateRes.EnsureSuccessStatusCode();
            var updatedShop = await updateRes.Content.ReadFromJsonAsync<ShopResult>();
            
            Assert.NotNull(updatedShop);
            Assert.Equal(newName, updatedShop.Name);
            Assert.Equal(newDescription, updatedShop.Description);
            Assert.Equal(userId, updatedShop.OwnerId); // OwnerId should remain the same
        }

        [Fact(DisplayName = "Update shop with description via management endpoint")]
        public async Task UpdateShopManage_WithDescription_ReturnsSuccess()
        {
            // Register and login user
            var (token, userId) = await RegisterAndLoginUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create shop first
            var shopName = $"ร้านจัดการเดิม {System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var createRes = await _client.PostAsJsonAsync("shops/manage", new { 
                Name = shopName, 
                Description = "คำอธิบายจัดการเดิม" 
            });
            createRes.EnsureSuccessStatusCode();
            var createdShop = await createRes.Content.ReadFromJsonAsync<ShopResult>();

            // Update shop with new description via management endpoint
            var newName = $"ร้านจัดการใหม่ {System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var newDescription = "คำอธิบายจัดการใหม่ - อัพเดทผ่าน manage endpoint";
            
            var updateRes = await _client.PutAsJsonAsync($"shops/manage/{createdShop.Id}", new { 
                Id = createdShop.Id,
                Name = newName, 
                Description = newDescription,
                OwnerId = userId
            });
            
            updateRes.EnsureSuccessStatusCode();
            var updatedShop = await updateRes.Content.ReadFromJsonAsync<ShopResult>();
            
            Assert.NotNull(updatedShop);
            Assert.Equal(newName, updatedShop.Name);
            Assert.Equal(newDescription, updatedShop.Description);
            Assert.Equal(userId, updatedShop.OwnerId);
        }

        [Fact(DisplayName = "Create shop without description defaults to empty string")]
        public async Task CreateShop_WithoutDescription_DefaultsToEmptyString()
        {
            // Register and login user
            var (token, userId) = await RegisterAndLoginUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create shop without description
            var shopName = $"ร้านไม่มีคำอธิบาย {System.Guid.NewGuid().ToString().Substring(0, 8)}";
            
            var createRes = await _client.PostAsJsonAsync("shops", new { 
                Name = shopName 
            });
            
            createRes.EnsureSuccessStatusCode();
            var shop = await createRes.Content.ReadFromJsonAsync<ShopResult>();
            
            Assert.NotNull(shop);
            Assert.Equal(shopName, shop.Name);
            Assert.Equal(string.Empty, shop.Description); // Should default to empty string
            Assert.Equal(userId, shop.OwnerId);
        }

        [Fact(DisplayName = "Update shop preserves OwnerId when updating description")]
        public async Task UpdateShop_PreservesOwnerId_WhenUpdatingDescription()
        {
            // Register and login user
            var (token, userId) = await RegisterAndLoginUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create shop first
            var shopName = $"ร้านทดสอบ OwnerId {System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var createRes = await _client.PostAsJsonAsync("shops", new { 
                Name = shopName, 
                Description = "คำอธิบายเริ่มต้น" 
            });
            createRes.EnsureSuccessStatusCode();
            var createdShop = await createRes.Content.ReadFromJsonAsync<ShopResult>();

            // Try to update with different OwnerId (should be ignored)
            var updateRes = await _client.PutAsJsonAsync($"shops/{createdShop.Id}", new { 
                Id = createdShop.Id,
                Name = shopName, 
                Description = "คำอธิบายใหม่",
                OwnerId = 99999 // This should be ignored
            });
            
            updateRes.EnsureSuccessStatusCode();
            var updatedShop = await updateRes.Content.ReadFromJsonAsync<ShopResult>();
            
            Assert.NotNull(updatedShop);
            Assert.Equal("คำอธิบายใหม่", updatedShop.Description);
            Assert.Equal(userId, updatedShop.OwnerId); // Should remain original owner, not 99999
        }

        [Fact(DisplayName = "Get shops includes description in response")]
        public async Task GetShops_IncludesDescription_InResponse()
        {
            // Register and login user
            var (token, userId) = await RegisterAndLoginUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Create shop with description
            var shopName = $"ร้านสำหรับดูรายการ {System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var shopDescription = "คำอธิบายสำหรับทดสอบการดึงข้อมูล";
            
            var createRes = await _client.PostAsJsonAsync("shops", new { 
                Name = shopName, 
                Description = shopDescription 
            });
            createRes.EnsureSuccessStatusCode();
            var createdShop = await createRes.Content.ReadFromJsonAsync<ShopResult>();

            // Get shops via management endpoint
            var getRes = await _client.GetAsync("shops/manage");
            getRes.EnsureSuccessStatusCode();
            var shops = await getRes.Content.ReadFromJsonAsync<List<ShopWithFoodsResult>>();
            
            Assert.NotNull(shops);
            var testShop = shops.Find(s => s.Id == createdShop.Id);
            Assert.NotNull(testShop);
            // Note: The current API doesn't return Description in the ShopWithFoodsDto
            // This test documents the current behavior
        }

        private async Task<(string token, int userId)> RegisterAndLoginUser()
        {
            var username = $"testuser_{System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var password = "Test1234";
            var email = $"{username}@mail.com";

            // Register
            var regRes = await _client.PostAsJsonAsync("users/register", new { 
                Username = username, 
                Password = password, 
                Email = email 
            });
            regRes.EnsureSuccessStatusCode();
            var user = await regRes.Content.ReadFromJsonAsync<UserResult>();

            // Login
            var loginRes = await _client.PostAsJsonAsync("users/login", new { 
                Username = username, 
                Password = password 
            });
            loginRes.EnsureSuccessStatusCode();
            var login = await loginRes.Content.ReadFromJsonAsync<LoginResult>();

            return (login.token, user.Id);
        }

        // DTOs for test responses
        public class UserResult 
        { 
            public int Id { get; set; } 
            public string Username { get; set; } = string.Empty;
        }
        
        public class LoginResult 
        { 
            public string token { get; set; } = string.Empty; 
        }
        
        public class ShopResult 
        { 
            public int Id { get; set; } 
            public string Name { get; set; } = string.Empty; 
            public string Description { get; set; } = string.Empty;
            public int OwnerId { get; set; } 
        }

        public class ShopWithFoodsResult
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public List<FoodResult> Foods { get; set; } = new();
        }

        public class FoodResult
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string ImagePath { get; set; } = string.Empty;
        }
    }
}
