using Microsoft.EntityFrameworkCore;
using WebApiServer.Data;
using WebApiServer.Interfaces;
using WebApiServer.Models;

namespace WebApiServer.Repository
{
    public class ProductRepository : IProduct
    {
        public DataContext _context { get; private set; }

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var result = await _context.Products.FindAsync(id);
            return result;
        }

        public async Task<List<Product>> GetProducts()
        {
            var result = await _context.Products.ToListAsync();
            return result;
        }

        public async Task<Product> Creeate(Product obj)
        {
            await _context.Products.AddAsync(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<Product> Update(Product obj)
        {
            var product = new Product()
            {
                Id = obj.Id,
                Name = obj.Name,
                Price = obj.Price,
                Description = obj.Description,
                Title = obj.Title,
                BrandId = obj.BrandId,
                ImgUrl = obj.ImgUrl
            };
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Delete(Product obj)
        {
            _context.Products.Remove(obj);
            await _context.SaveChangesAsync();
        }
    }
}