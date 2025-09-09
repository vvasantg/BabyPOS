using BabyPOS_Web2.Application.DTOs;
using BabyPOS_Web2.Application.Services;
using System.ComponentModel;

namespace BabyPOS_Web2.Presentation.ViewModels
{
    public class OrderManagementViewModel : INotifyPropertyChanged
    {
        private readonly IOrderService _orderService;
        private List<OrderDto> _pendingOrders = new();
        private List<OrderDto> _readyOrders = new();
        private bool _isLoading = false;
        private string _activeTab = "pending";

        public OrderManagementViewModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public List<OrderDto> PendingOrders
        {
            get => _pendingOrders;
            set
            {
                _pendingOrders = value;
                OnPropertyChanged(nameof(PendingOrders));
            }
        }

        public List<OrderDto> ReadyOrders
        {
            get => _readyOrders;
            set
            {
                _readyOrders = value;
                OnPropertyChanged(nameof(ReadyOrders));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string ActiveTab
        {
            get => _activeTab;
            set
            {
                _activeTab = value;
                OnPropertyChanged(nameof(ActiveTab));
            }
        }

        public async Task LoadOrdersAsync(int shopId)
        {
            IsLoading = true;
            try
            {
                var pendingTask = _orderService.GetPendingOrdersAsync(shopId);
                var readyTask = _orderService.GetReadyOrdersAsync(shopId);

                await Task.WhenAll(pendingTask, readyTask);

                PendingOrders = await pendingTask;
                ReadyOrders = await readyTask;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<bool> MarkOrderAsReadyAsync(int orderId)
        {
            var result = await _orderService.MarkOrderAsReadyAsync(orderId);
            if (result)
            {
                // Move order from pending to ready
                var order = PendingOrders.FirstOrDefault(o => o.Id == orderId);
                if (order != null)
                {
                    order.Status = "Ready";
                    PendingOrders = PendingOrders.Where(o => o.Id != orderId).ToList();
                    ReadyOrders = ReadyOrders.Concat(new[] { order }).ToList();
                }
            }
            return result;
        }

        public async Task<bool> MarkOrderAsCompletedAsync(int orderId)
        {
            var result = await _orderService.MarkOrderAsCompletedAsync(orderId);
            if (result)
            {
                // Remove order from ready list
                ReadyOrders = ReadyOrders.Where(o => o.Id != orderId).ToList();
            }
            return result;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
