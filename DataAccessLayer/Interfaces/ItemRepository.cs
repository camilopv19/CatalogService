using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

        public IEnumerable<ItemWithCategory> GetItemsWithCategory()
        {
            var items = List();
            var itemsList = new List<ItemWithCategory>();

            // Fetch categories for each item
            foreach (var item in items)
            {
                ItemWithCategory itemWithCategory = new ItemWithCategory(item);
                itemWithCategory.Category = _dbContext.Categories.Where(category => category.Id == item.CategoryId).FirstOrDefault() ?? new Category();
                itemsList.Add(itemWithCategory);
            }

            return itemsList;
        }
        public Item? Get(int id)
        {
            return _dbContext.Items.Find(id);
        }
        public IEnumerable<ItemResponse> List(int? categoryId, int? page)
        {
            IQueryable<Item> items = _dbContext.Items;
            List<ItemResponse> itemsResponseList = new List<ItemResponse>();

            if (items.Any())
            {
                if (categoryId != null)
                {
                    items = items.Where(item => item.CategoryId == categoryId);
                }
                if (page != null)
                {
                    int itemsPerPage = 20;
                    int skipCount = (page.Value - 1) * itemsPerPage;
                    items = items.Skip(skipCount).Take(itemsPerPage);
                }
                foreach (var item in items)
                {
                    var itemResponse = new ItemResponse(item);

                    // Add self-link to the current item
                    itemResponse.Links.Add(new LinkDto($"/api/items/{item.Id}", "self", "GET"));

                    // You can also add links to related resources, e.g., categories
                    itemResponse.Links.Add(new LinkDto($"/api/categories/{item.CategoryId}", "category", "GET"));
                    itemsResponseList.Add(itemResponse);
                }
            }
            return itemsResponseList;
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
