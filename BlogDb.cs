using Microsoft.EntityFrameworkCore;

namespace BlogApi
{
    public class BlogDb : DbContext
    {
        public BlogDb(DbContextOptions<BlogDb> options) : base(options)
        {}

        
        public DbSet<Post> Posts => Set<Post>();   

    }
}
