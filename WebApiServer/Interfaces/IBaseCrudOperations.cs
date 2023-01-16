using WebApiServer.Models;

namespace WebApiServer.Interfaces
{
    public interface IBaseCrudOperations<T>
    {
        public Task<T> Creeate(T obj);
        public Task<T> Update(T obj);
        public Task Delete(T obj);
    }
}