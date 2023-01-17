using WebApiServer.Models;

namespace WebApiServer.Interfaces
{
    public interface IBrand : IBaseCrudOperations<Brand>
    {
        public Task<Brand> GetBrandById(Guid id);
        public Task<List<Brand>> GetProducts();
    }
}