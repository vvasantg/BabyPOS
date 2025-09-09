using BabyPOS_API.Models;

namespace BabyPOS_API.Application.Services
{
    public interface ITableService
    {
        Task<Table> CreateTableAsync(Table table);
        Task<IEnumerable<Table>> GetTablesByShopAsync(int shopId);
        Task<Table?> GetTableAsync(int id);
        Task<Table?> UpdateTableAsync(int id, Table table);
        Task<bool> DeleteTableAsync(int id);
    }
}
