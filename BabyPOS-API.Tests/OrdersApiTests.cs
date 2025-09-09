using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using BabyPOS_API.Data;
using BabyPOS_API.Models;

namespace BabyPOS_API.Tests
{
    public class OrdersApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public OrdersApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove all database-related services
                    var dbContextDescriptors = services.Where(d => 
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                        d.ServiceType == typeof(AppDbContext) ||
                        d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>) ||
                        d.ImplementationType?.FullName?.Contains("Npgsql") == true)
                        .ToList();
                    
                    foreach (var desc in dbContextDescriptors)
                    {
                        services.Remove(desc);
                    }

                    // Add in-memory database for testing with unique name per test
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase($"TestDb_Orders_{Guid.NewGuid()}");
                    });
                });
                
                // Also remove the environment variable that points to PostgreSQL
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var memoryConfigData = new Dictionary<string, string?>
                    {
                        { "ConnectionStrings:DefaultConnection", "" }
                    };
                    config.AddInMemoryCollection(memoryConfigData);
                });
            });
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetAllOrders_ReturnsEmptyList_WhenNoOrders()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Act
            var response = await _client.GetAsync("/api/orders");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(orders);
            Assert.Empty(orders);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsAllOrders_WhenOrdersExist()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Create test shop and table
            var shop = new Shop { Id = 1, Name = "Test Shop" };
            var table = new Table { Id = 1, Name = "Table 1", ShopId = 1 };
            context.Shops.Add(shop);
            context.Tables.Add(table);

            // Create test orders
            var order1 = new Order
            {
                Id = 1,
                TableId = 1,
                Status = "Pending",
                ServiceType = "Dine-in",
                CreatedAt = DateTime.UtcNow.AddMinutes(-30)
            };

            var order2 = new Order
            {
                Id = 2,
                TableId = null,
                Status = "Preparing",
                ServiceType = "Takeaway",
                CreatedAt = DateTime.UtcNow.AddMinutes(-15)
            };

            context.Orders.AddRange(order1, order2);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/orders");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(orders);
            Assert.Equal(2, orders.Count);
            Assert.Contains(orders, o => o.Status == "Pending" && o.ServiceType == "Dine-in");
            Assert.Contains(orders, o => o.Status == "Preparing" && o.ServiceType == "Takeaway");
        }

        [Fact]
        public async Task GetOrdersByShop_ReturnsEmptyList_WhenNoOrdersForShop()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Create test shop
            var shop = new Shop { Id = 1, Name = "Test Shop" };
            context.Shops.Add(shop);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/orders/shop/1");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(orders);
            Assert.Empty(orders);
        }

        [Fact]
        public async Task GetOrdersByShop_ReturnsOrdersForSpecificShop_WhenOrdersExist()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Create test shops and tables
            var shop1 = new Shop { Id = 1, Name = "Shop 1" };
            var shop2 = new Shop { Id = 2, Name = "Shop 2" };
            var table1 = new Table { Id = 1, Name = "Table 1", ShopId = 1 };
            var table2 = new Table { Id = 2, Name = "Table 2", ShopId = 2 };

            context.Shops.AddRange(shop1, shop2);
            context.Tables.AddRange(table1, table2);

            // Create orders for different shops
            var orderShop1 = new Order
            {
                Id = 1,
                TableId = 1,
                Status = "Pending",
                ServiceType = "Dine-in",
                CreatedAt = DateTime.UtcNow.AddMinutes(-30)
            };

            var orderShop2 = new Order
            {
                Id = 2,
                TableId = 2,
                Status = "Ready",
                ServiceType = "Takeaway",
                CreatedAt = DateTime.UtcNow.AddMinutes(-15)
            };

            var anotherOrderShop1 = new Order
            {
                Id = 3,
                TableId = null,
                Status = "Preparing",
                ServiceType = "Delivery",
                CreatedAt = DateTime.UtcNow.AddMinutes(-10)
            };

            context.Orders.AddRange(orderShop1, orderShop2, anotherOrderShop1);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/orders/shop/1");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<Order>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(orders);
            Assert.Equal(2, orders.Count);
            Assert.All(orders, order => {
                // Check that order belongs to shop 1 through table relationship
                Assert.True(order.TableId == 1 || order.TableId == null);
            });
            Assert.Contains(orders, o => o.Status == "Pending" && o.ServiceType == "Dine-in");
            Assert.Contains(orders, o => o.Status == "Preparing" && o.ServiceType == "Delivery");
        }

        [Fact]
        public async Task GetOrdersByShop_ReturnsNotFound_WhenShopDoesNotExist()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Act
            var response = await _client.GetAsync("/api/orders/shop/999");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetOrdersByShop_OrdersIncludeTableInformation_WhenTableExists()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            // Create test data
            var shop = new Shop { Id = 1, Name = "Test Shop" };
            var table = new Table { Id = 1, Name = "Table A", ShopId = 1 };
            var menuItem = new MenuItem
            {
                Id = 1,
                Name = "Test Item",
                Price = 25.50m,
                ShopId = 1,
                Category = FoodCategory.MainDish,
                ImagePath = "test.jpg"
            };

            context.Shops.Add(shop);
            context.Tables.Add(table);
            context.MenuItems.Add(menuItem);

            var order = new Order
            {
                Id = 1,
                TableId = 1,
                Status = "Pending",
                ServiceType = "Dine-in",
                CreatedAt = DateTime.UtcNow.AddMinutes(-20)
            };

            var orderItem = new OrderItem
            {
                Id = 1,
                OrderId = 1,
                MenuItemId = 1,
                Quantity = 2,
                Price = 25.50m
            };

            context.Orders.Add(order);
            context.OrderItems.Add(orderItem);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/orders/shop/1");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            
            // Check that we can deserialize the response
            var orders = JsonSerializer.Deserialize<List<dynamic>>(content);
            Assert.NotNull(orders);
            Assert.Single(orders);
            
            // Verify the response contains order data
            var responseText = await response.Content.ReadAsStringAsync();
            Assert.Contains("Pending", responseText);
            Assert.Contains("Dine-in", responseText);
        }

        [Theory]
        [InlineData("Pending")]
        [InlineData("Preparing")]
        [InlineData("Ready")]
        [InlineData("Completed")]
        public async Task GetOrdersByShop_ReturnsOrdersWithDifferentStatuses(string status)
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var shop = new Shop { Id = 1, Name = "Test Shop" };
            context.Shops.Add(shop);

            var order = new Order
            {
                Id = 1,
                TableId = null,
                Status = status,
                ServiceType = "Takeaway",
                CreatedAt = DateTime.UtcNow
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/orders/shop/1");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            
            Assert.Contains(status, content);
            Assert.Contains("Takeaway", content);
        }
    }
}
