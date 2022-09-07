using DataAccess.Models;

namespace BusinessLogic.Repository
{
    public interface IItemRepository
    {
        Task AddAsync(Item item);
        Task DeleteAsync(int id);
        Task UpdateAsync(Item item);
        Task<Item>? GetAsync(int id);
        Task<List<Item>> ToListAsync();
        bool ItemExists(int id);

    }
}