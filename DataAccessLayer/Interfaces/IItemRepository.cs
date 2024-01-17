using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IItemRepository
    {
        int Delete(int id);
        Item? Get(int id);
        IEnumerable<ItemWithCategory> GetItemsWithCategory();
        IEnumerable<Item> List();
        IEnumerable<ItemResponse> List(int? categoryId, int? page);
        int Upsert(Item item);
    }
}