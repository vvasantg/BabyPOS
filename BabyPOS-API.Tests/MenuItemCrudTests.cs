using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using BabyPOS_API.Models;

namespace BabyPOS_API.Tests
{
    public class MenuItemCrudTests
    {

        [Fact(DisplayName = "Add 3 menu items to Alice's shop (shopId=1)")]
        public async Task AddThreeMenuItemsToAliceShop()
        {
            int shopId = 1;
            var menuList = new[] {
                new { Name = "ข้าวหมูแดง", Price = 55, Category = FoodCategory.MainDish },
                new { Name = "ชาเย็น", Price = 25, Category = FoodCategory.Drink },
                new { Name = "ลอดช่อง", Price = 35, Category = FoodCategory.Dessert }
            };
            foreach (var m in menuList)
            {
                var res = await _client.PostAsJsonAsync("menuitems", new { Name = m.Name, Price = m.Price, ShopId = shopId, Category = m.Category });
                res.EnsureSuccessStatusCode();
                var menuItem = await res.Content.ReadFromJsonAsync<MenuItemResult>();
                Assert.Equal(m.Name, menuItem.Name);
                Assert.Equal(shopId, menuItem.ShopId);
                Assert.Equal(m.Category, menuItem.Category);
            }
        }
        private readonly HttpClient _client;
        public MenuItemCrudTests()
        {
            _client = new HttpClient { BaseAddress = new System.Uri("http://localhost:5179/api/") };
        }


        [Fact(DisplayName = "Add new menu item to Alice's shop (shopId=1)")]
        public async Task AddMenuItemToAliceShop()
        {
            int shopId = 1;
            var createRes = await _client.PostAsJsonAsync("menuitems", new { Name = "ข้าวมันไก่", Price = 60, ShopId = shopId, Category = FoodCategory.MainDish });
            createRes.EnsureSuccessStatusCode();
            var menuItem = await createRes.Content.ReadFromJsonAsync<MenuItemResult>();
            Assert.Equal("ข้าวมันไก่", menuItem.Name);
            Assert.Equal(shopId, menuItem.ShopId);
            Assert.Equal(FoodCategory.MainDish, menuItem.Category);
        }


        [Fact(DisplayName = "CRUD MenuItem and update category")]
        public async Task CrudMenuItemAndUpdateCategory()
        {
            // ใช้ร้านของ alice (userId=10, shopId=1)
            int ownerId = 10;
            int shopId = 1;

            // 2. Create (เพิ่มอาหาร)
                var createRes = await _client.PostAsJsonAsync("menuitems", new { Name = "ข้าวผัด", Price = 50, ShopId = shopId, Category = FoodCategory.MainDish });
            createRes.EnsureSuccessStatusCode();
            var menuItem = await createRes.Content.ReadFromJsonAsync<MenuItemResult>();
            Assert.Equal(FoodCategory.MainDish, menuItem.Category);

            // 3. Read (ดึงข้อมูล)
            var getRes = await _client.GetAsync($"menuitems/{menuItem.Id}");
            getRes.EnsureSuccessStatusCode();
            var getItem = await getRes.Content.ReadFromJsonAsync<MenuItemResult>();
            Assert.Equal("ข้าวผัด", getItem.Name);
            Assert.Equal(FoodCategory.MainDish, getItem.Category);

            // 4. Update (เปลี่ยน category เป็น Dessert)
                var updateRes = await _client.PutAsJsonAsync($"menuitems/{menuItem.Id}", new { Id = menuItem.Id, Name = "ข้าวผัด", Price = 50, ShopId = shopId, Category = FoodCategory.Dessert });
            updateRes.EnsureSuccessStatusCode();
            var updated = await updateRes.Content.ReadFromJsonAsync<MenuItemResult>();
            Assert.Equal(FoodCategory.Dessert, updated.Category);

            // 5. Delete
            var delRes = await _client.DeleteAsync($"menuitems/{menuItem.Id}");
            delRes.EnsureSuccessStatusCode();
        }

        public class ShopResult { public int Id { get; set; } }
        public class MenuItemResult { public int Id { get; set; } public string Name { get; set; } = string.Empty; public decimal Price { get; set; } public int ShopId { get; set; } public FoodCategory Category { get; set; } }
        public enum FoodCategory { MainDish = 0, Dessert = 1, Drink = 2, Other = 3 }
    }
}
