using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnline.Models;

namespace ShopOnline.Interface
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> GetProductByIdWithListTagsAsync(int id);
        Task<Product> AddAsync(Product p);

        Task UpdateAsync(Product product);

        Task<Product> DeleteAsync(int id);

        Task<List<Product>> GetAllAsync();

        Task<List<Product>> GetListProductFilterAsync(string keyword, int page, int pageSize, string sort);

        Task SaveAsync();
    }
}
