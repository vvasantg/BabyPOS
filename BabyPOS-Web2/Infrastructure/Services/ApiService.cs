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
            // Convert to API format
            var apiRequest = new
            {
                ShopId = order.ShopId,
                TableId = order.TableId ?? 0, // Send 0 instead of null to avoid parsing issues
                ServiceType = order.ServiceType,
                Items = order.OrderItems.Select(item => new
                {
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity
                }).ToList()
            };

            return await SendRequestAsync<OrderDto>("api/orders", HttpMethod.Post, apiRequest);
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto statusUpdate)
        {
            var result = await SendRequestAsync<object>($"api/orders/{id}/status", HttpMethod.Put, statusUpdate);
            return result != null;
        }

        public async Task<List<OrderDto>> GetOrdersByShopAsync(int shopId)
        {
            var response = await _httpClient.GetAsync($"api/orders/shop/{shopId}");
            if (!response.IsSuccessStatusCode) return new List<OrderDto>();
            
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonString);
            
            var orders = new List<OrderDto>();
            foreach (var orderElement in jsonDoc.RootElement.EnumerateArray())
            {
                var orderDto = new OrderDto
                {
                    Id = orderElement.GetProperty("id").GetInt32(),
                    TableId = orderElement.TryGetProperty("tableId", out var tableIdProp) && !tableIdProp.ValueKind.Equals(JsonValueKind.Null) ? tableIdProp.GetInt32() : null,
                    TableName = orderElement.TryGetProperty("table", out var tableProp) && !tableProp.ValueKind.Equals(JsonValueKind.Null) && tableProp.TryGetProperty("name", out var tableNameProp) ? tableNameProp.GetString() : null,
                    CreatedAt = DateTime.Parse(orderElement.GetProperty("createdAt").GetString()!),
                    CheckedOutAt = orderElement.TryGetProperty("checkedOutAt", out var checkedOutProp) && !checkedOutProp.ValueKind.Equals(JsonValueKind.Null) ? DateTime.Parse(checkedOutProp.GetString()!) : null,
                    IsClosed = orderElement.GetProperty("isClosed").GetBoolean(),
                    Status = orderElement.GetProperty("status").GetString()!,
                    ServiceType = orderElement.GetProperty("serviceType").GetString()!,
                    OrderItems = new List<OrderItemDto>()
                };

                // Map order items
                if (orderElement.TryGetProperty("orderItems", out var orderItemsArray))
                {
                    foreach (var itemElement in orderItemsArray.EnumerateArray())
                    {
                        var orderItem = new OrderItemDto
                        {
                            Id = itemElement.GetProperty("id").GetInt32(),
                            OrderId = itemElement.GetProperty("orderId").GetInt32(),
                            MenuItemId = itemElement.GetProperty("menuItemId").GetInt32(),
                            MenuItemName = itemElement.TryGetProperty("menuItem", out var menuItemProp) && menuItemProp.TryGetProperty("name", out var nameProp) ? nameProp.GetString()! : "ไม่ระบุชื่อ",
                            Quantity = itemElement.GetProperty("quantity").GetInt32(),
                            Price = itemElement.TryGetProperty("menuItem", out var menuProp) && menuProp.TryGetProperty("price", out var priceProp) ? priceProp.GetDecimal() : itemElement.GetProperty("price").GetDecimal()
                        };
                        orderDto.OrderItems.Add(orderItem);
                    }
                }

                orders.Add(orderDto);
            }

            return orders;
        }

        public async Task<bool> CloseOrderAsync(int orderId)
        {
            var result = await SendRequestAsync<object>($"api/orders/{orderId}/close", HttpMethod.Put);
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

        public async Task<MenuItemDto?> CreateMenuItemAsync(CreateMenuItemDto menuItem)
        {
            return await SendRequestAsync<MenuItemDto>("api/menuitems", HttpMethod.Post, menuItem, requiresAuth: true);
        }

        public async Task<MenuItemDto?> UpdateMenuItemAsync(int id, CreateMenuItemDto menuItem)
        {
            return await SendRequestAsync<MenuItemDto>($"api/menuitems/{id}", HttpMethod.Put, menuItem, requiresAuth: true);
        }

        public async Task<bool> DeleteMenuItemAsync(int id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"api/menuitems/{id}");
                
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.SendAsync(request);
                
                // 204 NoContent หมายถึงลบสำเร็จ
                // 200 OK ก็ได้เหมือนกัน
                return response.StatusCode == System.Net.HttpStatusCode.NoContent || 
                       response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Exception deleting menu item {id}: {ex.Message}");
                return false;
            }
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
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"api/shops/{id}");
                
                // Add JWT token if available
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    Console.WriteLine("🔐 Added JWT token to request");
                }

                Console.WriteLine($"🚀 API Request: DELETE {_httpClient.BaseAddress}api/shops/{id}");

                var response = await _httpClient.SendAsync(request);
                
                Console.WriteLine($"📥 Delete Response Status: {response.StatusCode} ({(int)response.StatusCode}) for api/shops/{id}");

                // For DELETE requests, we check if the status code indicates success
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Exception in DeleteShopAsync: {ex.Message}");
                return false;
            }
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

        public async Task<TableDto?> GetTableAsync(int id)
        {
            return await SendRequestAsync<TableDto>($"api/tables/{id}", HttpMethod.Get);
        }

        public async Task<TableDto?> CreateTableAsync(CreateTableDto table)
        {
            return await SendRequestAsync<TableDto>("api/tables", HttpMethod.Post, table);
        }

        public async Task<TableDto?> UpdateTableAsync(int id, CreateTableDto table)
        {
            return await SendRequestAsync<TableDto>($"api/tables/{id}", HttpMethod.Put, table);
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"api/tables/{id}");
                
                // Add JWT token if available
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    Console.WriteLine("🔐 Added JWT token to request");
                }

                Console.WriteLine($"🚀 API Request: DELETE {_httpClient.BaseAddress}api/tables/{id}");

                var response = await _httpClient.SendAsync(request);
                
                Console.WriteLine($"📥 Delete Response Status: {response.StatusCode} ({(int)response.StatusCode}) for api/tables/{id}");

                // For DELETE requests, we check if the status code indicates success
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Exception in DeleteTableAsync: {ex.Message}");
                return false;
            }
        }

        // Bills Management
        public async Task<List<BillDto>> GetBillsByShopAsync(int shopId)
        {
            return await SendRequestAsync<List<BillDto>>($"api/bills/shop/{shopId}", HttpMethod.Get) ?? new List<BillDto>();
        }

        public async Task<BillDto?> GetBillAsync(int id)
        {
            return await SendRequestAsync<BillDto>($"api/bills/{id}", HttpMethod.Get);
        }

        public async Task<GenerateBillsDto?> GenerateBillsAsync(int shopId)
        {
            return await SendRequestAsync<GenerateBillsDto>($"api/bills/generate/{shopId}", HttpMethod.Post);
        }

        public async Task<bool> PayBillAsync(int billId)
        {
            var result = await SendRequestAsync<object>($"api/bills/{billId}/pay", HttpMethod.Put);
            return result != null;
        }

        public async Task<bool> SubmitOrderAsync(object orderData)
        {
            var result = await SendRequestAsync<object>("api/orders", HttpMethod.Post, orderData);
            return result != null;
        }
    }
}
