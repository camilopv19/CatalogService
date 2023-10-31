using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Interfaces
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Category> List()
        {
            return _dbContext.Categories;
        }
        public Category? Get(int id)
        {
            return _dbContext.Categories.Find(id);
        }

        public int Upsert(Category category)
        {
            var isNewCategory = !_dbContext.Categories.Any(x => x.Id == category.Id);
            if (isNewCategory)
            {
                _dbContext.Categories.Add(category);
            }
            else
            {
                _dbContext.Categories.Update(category);
            }
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {

            var category = _dbContext.Categories.Find(id);
            if (category != null)
            {
                var itemsToDelete = _dbContext.Items.Where(i => i.CategoryId == id);
                _dbContext.Items.RemoveRange(itemsToDelete);
                _dbContext.Categories.Remove(category);
            }
            return _dbContext.SaveChanges();
        }

    }
}
