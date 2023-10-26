using Microsoft.EntityFrameworkCore;
using ShopOnline.Data;
using ShopOnline.Interface;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductCategory> AddAsync(ProductCategory productCategory)
        {
            await _context.ProductCategories.AddAsync(productCategory);
            await _context.SaveChangesAsync();
            return productCategory;
        }

        public async Task<ProductCategory> DeleteAsync(int id)
        {
            var productCategory = await _context.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return null;
            }

            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();
            return productCategory;
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync(string keyword)
        {
            var query = _context.ProductCategories.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(pc => pc.Name.Contains(keyword) || pc.Description.Contains(keyword));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetAllByParentIdAsync(int parentId)
        {
            return await _context.ProductCategories.Where(pc => pc.ParentID == parentId).ToListAsync();
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await _context.ProductCategories.FindAsync(id);
        }
        public ProductCategory GetById(int id)
        {
            return  _context.ProductCategories.Where(x => x.ID == id).FirstOrDefault();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductCategory productCategory)
        {
            _context.Entry(productCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
