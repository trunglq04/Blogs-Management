using NetAng.API.Models.Domain;

namespace NetAng.API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);

        Task<IEnumerable<Category>> GetAllAsync(
            string? query,
            string? sortBy,
            string? sortDirection,
            int? pageNumber,
            int? pageSize);

        Task<Category?> GetByIdAsync(Guid id);

        Task<Category?> UpdateAsync(Category category);

        Task<Category?> DeleteAsync(Guid id);

        Task<int> GetCount();
    }
}
