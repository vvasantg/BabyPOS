using BabyPOS_Web2.Application.DTOs;
using System.Text.Json;
using System.Text;

namespace BabyPOS_Web2.Infrastructure.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        private async Task<T?> SendRequestAsync<T>(string endpoint, HttpMethod method, object? body = null)
        {
            try
            {
                Console.WriteLine($"🚀 API Request: {method} {endpoint}");
                
                var request = new HttpRequestMessage(method, endpoint);
                if (body != null)
                {
                    var json = JsonSerializer.Serialize(body, _jsonOptions);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    Console.WriteLine($"📤 Request Body: {json}");
                }

                var response = await _httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"📥 Response Status: {response.StatusCode} ({(int)response.StatusCode})");
                Console.WriteLine($"📥 Response Content: {(content.Length > 500 ? content.Substring(0, 500) + "..." : content)}");

                if (response.IsSuccessStatusCode)
                {
                    if (string.IsNullOrEmpty(content)) return default(T);
                    return JsonSerializer.Deserialize<T>(content, _jsonOptions);
                }
                else
                {
                    Console.WriteLine($"❌ API Error: {response.StatusCode} - {content}");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Exception in {endpoint}: {ex.Message}");
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
