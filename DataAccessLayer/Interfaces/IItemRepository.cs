using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IItemRepository
    {
        int Delete(int id);
        Item? Get(int id);
        IEnumerable<Item> List();
        int Upsert(Item item);
    }
}