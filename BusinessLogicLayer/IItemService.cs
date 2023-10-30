using DataAccessLayer.Entities;

namespace BusinessLogicLayer
{
    public interface IItemService
    {
        int Delete(int id);
        Item? Get(int id);
        IEnumerable<ItemResponse> List(int? categoryId, int? page);
        int Upsert(Item item);
    }
}