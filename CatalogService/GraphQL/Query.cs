using BusinessLogicLayer.CoreLogic;
using DataAccessLayer.Entities;

namespace CatalogService.GraphQL
{
    public class Query
    {
        private readonly ICategoryService _categoryService;

        public Query(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return _categoryService.List();
        }
    }
}
