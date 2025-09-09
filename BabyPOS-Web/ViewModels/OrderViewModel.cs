using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BabyPOS_Web.Models;

namespace BabyPOS_Web.ViewModels
{
    public class OrderViewModel
    {
        private readonly HttpClient _http;
        public Order? CurrentOrder { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public OrderViewModel(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/api/orders", order);
                if (response.IsSuccessStatusCode)
                {
                    CurrentOrder = await response.Content.ReadFromJsonAsync<Order>();
                    return true;
                }
                ErrorMessage = "Create order failed.";
                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }
    }
}
