using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface ICategoryRepository
    {
        int Delete(int id);
        Category? Get(int id);
        IEnumerable<Category> List();
        int Upsert(Category item);
    }
}