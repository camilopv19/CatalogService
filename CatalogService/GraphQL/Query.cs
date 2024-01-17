using BusinessLogicLayer.CoreLogic;
using DataAccessLayer.Entities;

namespace CatalogService.GraphQL
{
    public class Query
    {
        private readonly IItemService _itemService;

        public Query(IItemService itemService)
        {
            _itemService = itemService;
        }
        public IEnumerable<ItemWithCategory> GetItems()
        {
            return _itemService.GetItemsWithCategory();
        }
    }
}
