using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace BabyPOS_API.Tests
{
    public class MenuItemsApiTests
    {
        private readonly string _baseUrl = "http://localhost:5179/api/menuitems";
        private readonly string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI5IiwidW5pcXVlX25hbWUiOiJ2YXMiLCJleHAiOjE3NTc3NTgwMTQsImlzcyI6IkJhYnlQT1MiLCJhdWQiOiJCYWJ5UE9TVXNlcnMifQ.SfYHzB6VmFlIQQcnAGPJf8SxJEr4gyVDfv9Uo_QrgNE";

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            return client;
        }

        [Fact]
        public async Task Can_CRU_MenuItem()
        {
            var client = GetClient();
            // Create
            var createDto = new { Name = "UnitTestFood", Price = 99, ShopId = 1, Category = 0, ImagePath = "/img/test.jpg" };
            var createResp = await client.PostAsJsonAsync(_baseUrl, createDto);
            createResp.EnsureSuccessStatusCode();
            var created = await createResp.Content.ReadFromJsonAsync<MenuItemDto>();
            Assert.NotNull(created);
            Assert.Equal("UnitTestFood", created.Name);

            // Read
            var getResp = await client.GetAsync($"{_baseUrl}/{created.Id}");
            getResp.EnsureSuccessStatusCode();
            var fetched = await getResp.Content.ReadFromJsonAsync<MenuItemDto>();
            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched.Id);

            // Update
            var updateDto = new { Id = created.Id, Name = "UnitTestFoodUpdated", Price = 120, ShopId = created.ShopId, Category = 0, ImagePath = "/img/test2.jpg" };
            var updateResp = await client.PutAsJsonAsync($"{_baseUrl}/{created.Id}", updateDto);
            updateResp.EnsureSuccessStatusCode();
            var updated = await updateResp.Content.ReadFromJsonAsync<MenuItemDto>();
            Assert.NotNull(updated);
            Assert.Equal("UnitTestFoodUpdated", updated.Name);
        }

        public class MenuItemDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int ShopId { get; set; }
            public string? ImagePath { get; set; }
            public int Category { get; set; } // enum as int
        }
    }
}
