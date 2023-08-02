using Microsoft.EntityFrameworkCore;
using ShopOnline.Data;

namespace ShopOnline.Infrastructure
{
    public class DbFactory : Disposable
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private ApplicationDbContext context;

        public DbFactory(DbContextOptions<ApplicationDbContext> options)
        {
            _options = options;
        }

        public ApplicationDbContext Init()
        {
            return context ?? (context = new ApplicationDbContext(_options));
        }

        protected override void DisposeCore()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}
