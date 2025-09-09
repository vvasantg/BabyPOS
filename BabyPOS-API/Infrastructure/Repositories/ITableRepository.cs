using BabyPOS_API.Models;

namespace BabyPOS_API.Infrastructure.Repositories
{
    public interface ITableRepository
    {
        Task<Table> AddAsync(Table table);
        Task<IEnumerable<Table>> GetByShopAsync(int shopId);
        Task<Table?> GetByIdAsync(int id);
        Task<Table?> UpdateAsync(int id, Table table);
        Task<bool> DeleteAsync(int id);
    }
}
