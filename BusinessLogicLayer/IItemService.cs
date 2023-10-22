using DataAccessLayer.Entities;

namespace BusinessLogicLayer
{
    public interface IItemService
    {
        int Delete(int id);
        Item? Get(int id);
        IEnumerable<Item> List();
        int Upsert(Item item);
    }
}