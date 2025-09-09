using BabyPOS_Web2.Application.DTOs;
using System.Text.Json;
using System.Text;
using Microsoft.JSInterop;
using System.Linq;

namespace BabyPOS_Web2.Infrastructure.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IJSRuntime _jsRuntime;

        public ApiService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        private async Task<T?> SendRequestAsync<T>(string endpoint, HttpMethod method, object? body = null, bool requiresAuth = false)
        {
            try
            {
                var fullUrl = $"{_httpClient.BaseAddress?.ToString().TrimEnd('/')}/{endpoint.TrimStart('/')}";
                Console.WriteLine($"🚀 API Request: {method} {fullUrl}");
                
                var request = new HttpRequestMessage(method, endpoint);
                
                // Add JWT token for authenticated requests
                if (requiresAuth)
                {
                    try
                    {
                        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
                        if (!string.IsNullOrEmpty(token))
                        {
                            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                            Console.WriteLine($"🔐 Added JWT token to request");
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ No JWT token found for authenticated request");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error getting JWT token: {ex.Message}");
                    }
                }
                
                if (body != null)
                {
                    var json = JsonSerializer.Serialize(body, _jsonOptions);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    Console.WriteLine($"📤 Request Body: {json}");
                }

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"📥 Response Status: {response.StatusCode} ({(int)response.StatusCode}) for {endpoint}");
                Console.WriteLine($"📥 Response Content: {(content.Length > 500 ? content.Substring(0, 500) + "..." : content)}");

                if (response.IsSuccessStatusCode)
                {
                    if (string.IsNullOrEmpty(content)) return default(T);
                    return JsonSerializer.Deserialize<T>(content, _jsonOptions);
                }
                else
                {
                    Console.WriteLine($"❌ API Error for {endpoint}: {response.StatusCode} - {content}");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Exception calling {endpoint}: {ex.Message}");
                return default(T);
            }
        }

        public async Task<List<OrderDto>> GetOrdersAsync(int shopId)
        {
            var result = await SendRequestAsync<List<OrderDto>>($"api/orders?shopId={shopId}", HttpMethod.Get);
            return result ?? new List<OrderDto>();
        }

        public async Task<List<OrderDto>> GetOrdersByStatusAsync(int shopId, string status)
        {
            var result = await SendRequestAsync<List<OrderDto>>($"api/orders?shopId={shopId}&status={status}", HttpMethod.Get);
            return result ?? new List<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderAsync(int id)
        {
            return await SendRequestAsync<OrderDto>($"api/orders/{id}", HttpMethod.Get);
        }

        public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto order)
        {
            return await SendRequestAsync<OrderDto>("api/orders", HttpMethod.Post, order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto statusUpdate)
        {
            var result = await SendRequestAsync<object>($"api/orders/{id}/status", HttpMethod.Put, statusUpdate);
            return result != null;
        }

        public async Task<List<MenuItemDto>> GetMenuItemsAsync(int shopId)
        {
            var result = await SendRequestAsync<List<MenuItemDto>>($"api/menuitems/shop/{shopId}", HttpMethod.Get);
            return result ?? new List<MenuItemDto>();
        }

        public async Task<MenuItemDto?> GetMenuItemAsync(int id)
        {
            return await SendRequestAsync<MenuItemDto>($"api/menuitems/{id}", HttpMethod.Get);
        }

        public async Task<List<ShopDto>> GetShopsAsync()
        {
            var result = await SendRequestAsync<List<ShopDto>>("api/shops", HttpMethod.Get);
            return result ?? new List<ShopDto>();
        }

        public async Task<ShopDto?> GetShopAsync(int id)
        {
            return await SendRequestAsync<ShopDto>($"api/shops/{id}", HttpMethod.Get);
        }

        public async Task<ShopDto?> CreateShopAsync(ShopDto shop)
        {
            return await SendRequestAsync<ShopDto>("api/shops", HttpMethod.Post, shop, requiresAuth: true);
        }

        public async Task<bool> UpdateShopAsync(ShopDto shop)
        {
            var result = await SendRequestAsync<object>($"api/shops/{shop.Id}", HttpMethod.Put, shop, requiresAuth: true);
            return result != null;
        }

        public async Task<bool> DeleteShopAsync(int id)
        {
            var result = await SendRequestAsync<object>($"api/shops/{id}", HttpMethod.Delete, requiresAuth: true);
            return result != null;
        }

        public async Task<List<ShopDto>> GetShopsForManagementAsync()
        {
            var result = await SendRequestAsync<List<ShopWithFoodsDto>>("api/shops/manage", HttpMethod.Get, requiresAuth: true);
            if (result == null) return new List<ShopDto>();
            
            // Convert ShopWithFoodsDto to ShopDto
            return result.Select(shop => new ShopDto
            {
                Id = shop.Id,
                Name = shop.Name,
                Description = shop.Description,
                OwnerId = 0, // Will be set by API based on JWT
                OwnerName = null
            }).ToList();
        }

        public async Task<List<MenuItemDto>> GetShopMenuItemsAsync(int shopId)
        {
            return await GetMenuItemsAsync(shopId);
        }

        public async Task<List<TableDto>> GetTablesByShopAsync(int shopId)
        {
            var result = await SendRequestAsync<List<TableDto>>($"api/tables/shop/{shopId}", HttpMethod.Get);
            return result ?? new List<TableDto>();
        }

        public async Task<bool> SubmitOrderAsync(object orderData)
        {
            var result = await SendRequestAsync<object>("api/orders", HttpMethod.Post, orderData);
            return result != null;
        }
    }
}
