using System.Collections.Generic;
using System.Threading.Tasks;
using ShopOnline.Models;

namespace ShopOnline.Interface
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory> AddAsync(ProductCategory productCategory);

        Task UpdateAsync(ProductCategory productCategory);

        Task<ProductCategory> DeleteAsync(int id);

        Task<IEnumerable<ProductCategory>> GetAllAsync();

        Task<IEnumerable<ProductCategory>> GetAllAsync(string keyword);

        Task<IEnumerable<ProductCategory>> GetAllByParentIdAsync(int parentId);

        Task<ProductCategory> GetByIdAsync(int id);
        ProductCategory GetById(int id);
        Task SaveAsync();
    }
}

