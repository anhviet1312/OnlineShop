using ShopOnline.Models;
namespace ShopOnline.Interface
{
    public interface ITagRepository
    {
        Task<Tag> AddAsync(Tag tag);

        Task<Tag> DeleteAsync(string id);

        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag> GetByIdAsync(string id);

        Task SaveAsync();

        Task UpdateAsync(Tag tag);
    }
}
