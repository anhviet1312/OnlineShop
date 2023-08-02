using Microsoft.EntityFrameworkCore;
using ShopOnline.Data;
using ShopOnline.Interface;
using ShopOnline.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag> DeleteAsync(string id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
            {
                return null;
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(string id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag tag)
        {
            // No explicit update method in EF Core.
            // EF Core tracks changes to entities automatically when you retrieve them, so modifying the properties and calling SaveChangesAsync will update the entity in the database.
            await _context.SaveChangesAsync();
        }
    }
}
