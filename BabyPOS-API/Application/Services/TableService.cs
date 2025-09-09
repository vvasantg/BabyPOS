using BabyPOS_API.Models;
using BabyPOS_API.Infrastructure.Repositories;

namespace BabyPOS_API.Application.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _repo;
        public TableService(ITableRepository repo) { _repo = repo; }

        public Task<Table> CreateTableAsync(Table table) => _repo.AddAsync(table);
        public Task<IEnumerable<Table>> GetTablesByShopAsync(int shopId) => _repo.GetByShopAsync(shopId);
        public Task<Table?> GetTableAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Table?> UpdateTableAsync(int id, Table table) => _repo.UpdateAsync(id, table);
        public Task<bool> DeleteTableAsync(int id) => _repo.DeleteAsync(id);
    }
}
