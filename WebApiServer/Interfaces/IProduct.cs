using WebApiServer.Models;

namespace WebApiServer.Interfaces
{
    public interface IProduct : IBaseCrudOperations<Product>
    {
        public Task<Product> GetProductById(Guid id);
        public Task<List<Product>> GetProducts();
    }
}