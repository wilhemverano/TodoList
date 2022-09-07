using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext todoListContext;
        public ItemRepository(ApplicationDbContext todoListContext)
        {
            this.todoListContext = todoListContext;
        }
        public async Task AddAsync(Item item)
        {
            todoListContext.Items.Add(item);
            await todoListContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = todoListContext.Items.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                todoListContext.Remove(item);
                await todoListContext.SaveChangesAsync();
            }                
        }

        public async Task<Item>? GetAsync(int id)
        {
            var item = await todoListContext.Items.FirstOrDefaultAsync(x => x.Id == id);
            
            return item;
        }

        public async Task<List<Item>> ToListAsync()
        {
            return await todoListContext.Items.ToListAsync();
        }

        public async Task UpdateAsync(Item item)
        {            
            todoListContext.Update(item);                
            await todoListContext.SaveChangesAsync();            
        }

        public bool ItemExists(int id)
        {
            return (todoListContext.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
