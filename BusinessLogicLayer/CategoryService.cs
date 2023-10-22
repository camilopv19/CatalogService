using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<Category> List() => _repository.List();

        public Category? Get(int id) => _repository.Get(id);
        public int Delete(int id) => _repository.Delete(id);
        public int Upsert(Category category) => _repository.Upsert(category);
    }
}