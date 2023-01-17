using WebApiServer.Models;

namespace WebApiServer.Interfaces
{
    public interface ICategory : IBaseCrudOperations<Category>
    {
        public Task<Category> GetCategoryById(Guid id);
        public Task<List<Category>> GetCategories();
    }
}