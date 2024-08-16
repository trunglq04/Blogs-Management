using Microsoft.EntityFrameworkCore;
using NetAng.API.Data;
using NetAng.API.Models.Domain;
using NetAng.API.Repositories.Interface;

namespace NetAng.API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(category => category.Id == id);
            if (existingCategory is not null)
            {
                _context.Categories.Remove(existingCategory);
                await _context.SaveChangesAsync();
                return existingCategory;
            }

            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(category => category.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);

            if (existingCategory is not null)
            {
                _context.Entry(existingCategory).CurrentValues.SetValues(category); 
                await _context.SaveChangesAsync();
                return category;
            }

            return null;
        }
    }
}
