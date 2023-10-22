using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public class ItemRepository : IItemRepository
    {
        private readonly AppDbContext _dbContext;
        public ItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Item> List()
        {
            return _dbContext.Items;
        }
        public Item? Get(int id)
        {
            return _dbContext.Items.Find(id);
        }

        public int Upsert(Item item)
        {
            var isNewItem = !_dbContext.Items.Any(x => x.Id == item.Id);
            if (isNewItem)
            {
                _dbContext.Items.Add(item);
            }
            else
            {
                _dbContext.Items.Update(item);
            }
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var item = _dbContext.Items.Find(id);
            if (item != null)
            {
                _dbContext.Items.Remove(item);
            }
            return _dbContext.SaveChanges();
        }
    }
}
