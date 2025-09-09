using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;

namespace BabyPOS_API.Tests
{
    public class ShopHomeEndpointTests
    {
        private readonly HttpClient _client;
        public ShopHomeEndpointTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:5179/api/") };
        }

        [Fact(DisplayName = "GET /api/shops returns all shops with foods (ShopWithFoodsDto)")]
        public async Task GetShops_ReturnsAllShopsWithFoods()
        {
            // Arrange: create shop (no auth required)
            var shopName = $"ร้าน home_{System.Guid.NewGuid().ToString().Substring(0, 8)}";
            var shopRes = await _client.PostAsJsonAsync("shops", new { Name = shopName, OwnerId = 1 });
            shopRes.EnsureSuccessStatusCode();

            // Act
            var res = await _client.GetAsync("shops");
            res.EnsureSuccessStatusCode();
            var shops = await res.Content.ReadFromJsonAsync<List<ShopWithFoodsDto>>();

            // Assert
            Assert.NotNull(shops);
            Assert.Contains(shops, s => s.Name == shopName);
        }

        public class ShopWithFoodsDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public List<MenuItemDto> Foods { get; set; } = new();
        }
        public class MenuItemDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string? ImagePath { get; set; }
        }
    }
}
