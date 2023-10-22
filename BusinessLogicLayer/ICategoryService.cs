using DataAccessLayer.Entities;

namespace BusinessLogicLayer
{
    public interface ICategoryService
    {
        int Delete(int id);
        Category? Get(int id);
        IEnumerable<Category> List();
        int Upsert(Category category);
    }
}