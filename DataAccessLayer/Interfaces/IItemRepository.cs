using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IItemRepository
    {
        int Delete(int id);
        Item? Get(int id);
        IEnumerable<ItemResponse> List(int? categoryId, int? page);
        int Upsert(Item item);
    }
}