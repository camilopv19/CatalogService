using BusinessLogicLayer.CoreLogic;
using DataAccessLayer.Entities;

namespace CatalogService.GraphQL
{
    public class Query
    {
        private readonly ICategoryService _categoryService;
        private readonly IItemService _itemService;

        public Query(ICategoryService categoryService, IItemService itemService)
        {
            _categoryService = categoryService;
            _itemService = itemService;
        }


        public IEnumerable<Category> GetCategories()
        {
            return _categoryService.List();
        }
        public IEnumerable<Item> GetItems()
        {
            return _itemService.List();
        }
    }
}
