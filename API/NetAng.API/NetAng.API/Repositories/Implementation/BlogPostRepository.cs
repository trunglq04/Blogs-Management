using Microsoft.EntityFrameworkCore;
using NetAng.API.Data;
using NetAng.API.Models.Domain;
using NetAng.API.Repositories.Interface;

namespace NetAng.API.Repositories.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext context;

        public BlogPostRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await context.BlogPosts.AddAsync(blogPost); 
            await context.SaveChangesAsync();
            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            // From BlogPost and Categories table in DB,
            // we include the Categories base on BlogPost
            return await context.BlogPosts.Include(x => x.Categories).ToListAsync();
        }
    }
}
